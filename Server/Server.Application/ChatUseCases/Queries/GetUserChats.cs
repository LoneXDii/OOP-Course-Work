namespace Server.Application.ChatUseCases.Queries;

public sealed record GetUserChatsRequest(int userId) : IRequest<IEnumerable<Chat>> { }

internal class GetUserChatsRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetUserChatsRequest, IEnumerable<Chat>>
{
    public async Task<IEnumerable<Chat>> Handle(GetUserChatsRequest request, CancellationToken cancellationToken = default)
    {
        var chatIds = await unitOfWork.ChatMemberRepository.ListAsync(m => m.UserId == request.userId, cancellationToken);

        var chats = new List<Chat>();
        foreach (var chatId in chatIds)
        {
            var chat = await unitOfWork.ChatRepository.FirstOrDefaultAsync(c => c.Id == chatId.ChatId, cancellationToken);
            if (chat is not null)
            {
                chats.Add(chat);
            }
        }
        return chats;
    }
}

