namespace Server.Application.MessageUseCases.Commands;

public sealed record DeleteMessageRequest(int messageId) : IRequest { }

internal class DeleteMessageRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteMessageRequest>
{
    public async Task Handle(DeleteMessageRequest request, CancellationToken cancellationToken = default)
    {
        var message = await unitOfWork.MessageRepository.FirstOrDefaultAsync(m => m.Id == request.messageId);
        if (message is null) return;
        int chatId = 0;
        if (message.ChatId is not null)
        {
            chatId = message.ChatId.Value;
        }
        await unitOfWork.MessageRepository.DeleteAsync(message, cancellationToken);
        await unitOfWork.SaveAllAsync();

        if (chatId != 0)
        {
            var chat = await unitOfWork.ChatRepository.GetByIdAsync(chatId);
            if (chat is null)
            {
                return;
            }
            var messages = await unitOfWork.MessageRepository.ListAsync(m => m.ChatId == chatId, cancellationToken, m => m.User);

            try
            {
                var mess = messages.Last();
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
            catch {
                chat.LastMessageDate = DateTime.Now;
                chat.LastMessage = $"";
            }
            await unitOfWork.ChatRepository.UpdateAsync(chat);
            await unitOfWork.SaveAllAsync();
        }
    }
}

