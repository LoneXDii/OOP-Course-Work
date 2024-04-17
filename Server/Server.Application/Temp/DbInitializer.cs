using Microsoft.Extensions.DependencyInjection;
using Server.Domain.Entities;

namespace Server.Application.Temp;

public class DbInitializer
{
    public static async Task Initialize(IServiceProvider services)
    {
        var unitOfWork = services.GetRequiredService<IUnitOfWork>();

        await unitOfWork.DeleteDataBaseAsync();
        await unitOfWork.CreateDataBaseAsync();

        for (int i = 0; i < 10; i++)
        {
            var user = new User($"User {i}", $"User{i}", $"password{i}");
            user.AuthorizationToken = $"auth{1}";
            await unitOfWork.UserRepository.AddAsync(user);
        }
        await unitOfWork.SaveAllAsync();

        for (int i = 0; i < 2; i++)
        {
            var chat = new Chat($"Chat {i}");
            if (i == 0)
            {
                chat.CreatorId = Random.Shared.Next(1, 5);
            }
            else
            {
                chat.CreatorId = Random.Shared.Next(6, 10);
            }
            await unitOfWork.ChatRepository.AddAsync(chat);
        }
        await unitOfWork.SaveAllAsync();

        for (int i = 0; i < 100; i++)
        {
            var message = new Message($"message {i}");
            message.SenderId = Random.Shared.Next(1, 10);
            if (message.SenderId <= 5)
            {
                message.ChatId = 1;
            }
            else
            {
                message.ChatId = 2;
            }
            await unitOfWork.MessageRepository.AddAsync(message);
        }
        await unitOfWork.SaveAllAsync();

        for (int i = 0; i < 10; i++)
        {
            ChatMember member = new ChatMember(0, 0);
            if (i < 5)
            {
                member = new ChatMember(i + 1, 1);
            }
            else
            {
                member = new ChatMember(i + 1, 2);
            }
            await unitOfWork.ChatMemberRepository.AddAsync(member);
        }
        await unitOfWork.SaveAllAsync();
    }
}
