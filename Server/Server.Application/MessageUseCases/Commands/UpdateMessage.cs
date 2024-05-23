using Server.Domain.Entities;
using System.Net;

namespace Server.Application.MessageUseCases.Commands;

public sealed record UpdateMessageRequest(Message message) : IRequest<Message> { }

internal class UpdateMessageRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateMessageRequest, Message>
{
    public async Task<Message> Handle(UpdateMessageRequest request, CancellationToken cancellationToken = default)
    {
        await unitOfWork.MessageRepository.UpdateAsync(request.message, cancellationToken);
        await unitOfWork.SaveAllAsync();

        int chatId = 0;
        if (request.message.ChatId is not null)
        {
            chatId = request.message.ChatId.Value;
        }
        if (chatId != 0)
        {
            var chat = await unitOfWork.ChatRepository.GetByIdAsync(chatId);
            if (chat is null)
            {
                return request.message;
            }
            var mess = (await unitOfWork.MessageRepository.ListAsync(m => m.ChatId == chatId, cancellationToken, m => m.User))?.Last();
            if (mess is null)
            {
                chat.LastMessageDate = DateTime.Now;
                chat.LastMessage = $"";
            }
            else
            {
                chat.LastMessageDate = mess.SendTime;
                if (chat.IsDialogue)
                {
                    chat.LastMessage = $"{mess.Text}";
                }
                else
                {
                    chat.LastMessage = $"{mess.User.Name}: {mess.Text}";
                }
            }
            await unitOfWork.ChatRepository.UpdateAsync(chat);
            await unitOfWork.SaveAllAsync();
        }
        return request.message;
    }
}
