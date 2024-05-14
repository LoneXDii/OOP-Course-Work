using Microsoft.Extensions.DependencyInjection;
using Server.Domain.Entities;
using System.Security.Cryptography;
using System.Text;

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
            var password = $"password{i}";
            using SHA256 hash = SHA256.Create();
            password = Convert.ToHexString(hash.ComputeHash(Encoding.UTF8.GetBytes(password)));
            var user = new User($"User {i}", $"User{i}", password);
            user.AuthorizationToken = $"auth{1}";
            await unitOfWork.UserRepository.AddAsync(user);
        }
        await unitOfWork.SaveAllAsync();

        var a = unitOfWork.UserRepository;

        for (int i = 0; i < 2; i++)
        {
            var chat = new Chat($"Chat {i}");
            await unitOfWork.ChatRepository.AddAsync(chat);
        }
        await unitOfWork.SaveAllAsync();

        var b = unitOfWork.ChatRepository;

        for (int i = 0; i < 100; i++)
        {
            var message = new Message($"message {i}");
            int userId = Random.Shared.Next(1, 10);
            message.User = await unitOfWork.UserRepository.GetByIdAsync(userId);
            if (message.User.Id <= 5)
            {
                var chat = await unitOfWork.ChatRepository.GetByIdAsync(1);
                message.Chat = chat;
            }
            else
            {
                var chat = await unitOfWork.ChatRepository.GetByIdAsync(2);
                message.Chat = chat;
            }
            await unitOfWork.MessageRepository.AddAsync(message);
        }
        await unitOfWork.SaveAllAsync();

        for (int i = 1; i < 11; i++)
        {
            //ChatMember member = new ChatMember(0, 0);
            //if (i < 5)
            //{
            //    member = new ChatMember(i + 1, 1);
            //}
            //else
            //{
            //    member = new ChatMember(i + 1, 2);
            //}
            //await unitOfWork.ChatMemberRepository.AddAsync(member);
            if (i < 5)
            {
                var chat = await unitOfWork.ChatRepository.GetByIdAsync(1, default, c => c.Users);
                var user = await unitOfWork.UserRepository.GetByIdAsync(i);
                chat.Users.Add(user);
            }
            else
            {
                var chat = await unitOfWork.ChatRepository.GetByIdAsync(2, default, c => c.Users);
                var user = await unitOfWork.UserRepository.GetByIdAsync(i);
                chat.Users.Add(user);
            }
        }
        await unitOfWork.SaveAllAsync();
    }
}
