namespace Server.Application.MessageUseCases.Commands;

public sealed record AddMessageRequest(Message message) : IRequest<Message> { }

internal class AddMessageRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<AddMessageRequest, Message>
{
    public async Task<Message> Handle(AddMessageRequest request, CancellationToken cancellationToken = default)
    {
        await unitOfWork.MessageRepository.AddAsync(request.message, cancellationToken);
        if (request.message.ChatId is not null)
        {
            var chat = await unitOfWork.ChatRepository.GetByIdAsync(request.message.ChatId.Value, cancellationToken);
            if (chat is null)
            {
                await unitOfWork.SaveAllAsync();
                return request.message;
            }
            chat.LastMessageDate = request.message.SendTime;
            if (chat.IsDialogue)
            {
                chat.LastMessage = $"{request.message.Text}";
            }
            else
            {
                chat.LastMessage = $"{request.message.User.Name}: {request.message.Text}";
            }
            await unitOfWork.ChatRepository.UpdateAsync(chat);
        }
        await unitOfWork.SaveAllAsync();
        return request.message;
    }
}
