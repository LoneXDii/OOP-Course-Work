namespace Server.Application.MessageUseCases.Commands;

public sealed record UpdateMessageRequest(Message message) : IRequest<Message> { }

internal class UpdateMessageRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateMessageRequest, Message>
{
    public async Task<Message> Handle(UpdateMessageRequest request, CancellationToken cancellationToken)
    {
        await unitOfWork.MessageRepository.UpdateAsync(request.message, cancellationToken);
        await unitOfWork.SaveAllAsync();
        return request.message;
    }
}
