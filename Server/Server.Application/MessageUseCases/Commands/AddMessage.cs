namespace Server.Application.MessageUseCases.Commands;

public sealed record AddMessageRequest(Message message) : IRequest<Message> { }

internal class AddMessageRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<AddMessageRequest, Message>
{
    public async Task<Message> Handle(AddMessageRequest request, CancellationToken cancellationToken)
    {
        await unitOfWork.MessageRepository.AddAsync(request.message, cancellationToken);
        await unitOfWork.SaveAllAsync();
        return request.message;
    }
}
