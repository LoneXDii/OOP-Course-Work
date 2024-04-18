namespace Server.Application.ChatUseCases.Queries;

public sealed record GetChatMembersRequest(int chatId) : IRequest<IEnumerable<User>> { }

internal class GetChatMembersRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetChatMembersRequest, IEnumerable<User>>
{
    public async Task<IEnumerable<User>> Handle(GetChatMembersRequest request, CancellationToken cancellationToken = default)
    {
        //var members = await unitOfWork.ChatMemberRepository.ListAsync(m => m.ChatId == request.chatId, cancellationToken);
        //List<User> users = new();

        //foreach (var member in members)
        //{
        //    var user = await unitOfWork.UserRepository.FirstOrDefaultAsync(u => u.Id == member.UserId, cancellationToken);
        //    if (user is null) continue;
        //    users.Add(user);
        //}
        var chat = await unitOfWork.ChatRepository.GetByIdAsync(request.chatId, cancellationToken, c => c.Users);
        var users = chat.Users as IReadOnlyList<User>;
        return users;
    }
}
