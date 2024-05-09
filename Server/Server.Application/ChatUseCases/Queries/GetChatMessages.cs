namespace Server.Application.ChatUseCases.Queries;

public sealed record GetChatMessagesRequest(int chatId) : IRequest<IEnumerable<Message>> { }

internal class GetChatMessagesRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetChatMessagesRequest, IEnumerable<Message>>
{
    public async Task<IEnumerable<Message>> Handle(GetChatMessagesRequest request, CancellationToken cancellationToken = default)
    {
        var messages = await unitOfWork.MessageRepository.ListAsync(m => m.ChatId == request.chatId, cancellationToken, m => m.User);
        return messages;
    }
}

