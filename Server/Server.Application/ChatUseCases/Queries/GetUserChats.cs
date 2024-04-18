namespace Server.Application.ChatUseCases.Queries;

public sealed record GetUserChatsRequest(int userId) : IRequest<IEnumerable<Chat>> { }

internal class GetUserChatsRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetUserChatsRequest, IEnumerable<Chat>>
{
    public async Task<IEnumerable<Chat>> Handle(GetUserChatsRequest request, CancellationToken cancellationToken = default)
    {
        var user = await unitOfWork.UserRepository.GetByIdAsync(request.userId, cancellationToken, u => u.Chats);
        return user.Chats;
    }
}

