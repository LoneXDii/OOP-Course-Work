namespace Server.Application.MessageUseCases.Queries;

public sealed record GetMessageByIdRequest(int messageId) : IRequest<Message?> { }

internal class GetMessageByIdRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetMessageByIdRequest, Message?>
{
    public async Task<Message?> Handle(GetMessageByIdRequest request, CancellationToken cancellationToken = default)
    {
        return await unitOfWork.MessageRepository.FirstOrDefaultAsync(m => m.Id == request.messageId, cancellationToken);
    }
}
