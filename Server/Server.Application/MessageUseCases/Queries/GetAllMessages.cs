namespace Server.Application.MessageUseCases.Queries;

public sealed record GetAllMessagesRequest() : IRequest<IEnumerable<Message>> { }

internal class GetAllChatsRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllMessagesRequest, IEnumerable<Message>>
{
    public async Task<IEnumerable<Message>> Handle(GetAllMessagesRequest request, CancellationToken cancellationToken)
    {
        return await unitOfWork.MessageRepository.ListAllAsync(cancellationToken);
    }
}
