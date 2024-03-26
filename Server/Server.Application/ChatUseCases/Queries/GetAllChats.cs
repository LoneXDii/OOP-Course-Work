namespace Server.Application.ChatUseCases.Queries;

public sealed record GetAllChatsRequest() : IRequest<IEnumerable<Chat>> { }

internal class GetAllChatsRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllChatsRequest, IEnumerable<Chat>>
{
    public async Task<IEnumerable<Chat>> Handle(GetAllChatsRequest request, CancellationToken cancellationToken)
    {
        return await unitOfWork.ChatRepository.ListAllAsync(cancellationToken);
    }
}

