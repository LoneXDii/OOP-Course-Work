namespace Server.Application.ChatUseCases.Queries;

public sealed record GetUserChatMessagesRequest(int userId, int chatId) : IRequest<IEnumerable<Message>> { }

internal class GetUserChatMessagesRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetUserChatMessagesRequest, IEnumerable<Message>>
{
    public async Task<IEnumerable<Message>> Handle(GetUserChatMessagesRequest request, CancellationToken cancellationToken = default)
    {
        var messages = await unitOfWork.MessageRepository.ListAsync(m => m.SenderId == request.userId && m.ChatId == request.chatId
                                                                    , cancellationToken);
        return messages;
    }
}

