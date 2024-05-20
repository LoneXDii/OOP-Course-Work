namespace Server.Application.ChatUseCases.Commands;

public sealed record IsUserInChatRequest(int userId, int chatId) : IRequest<bool> { }

internal class IsUserInChatRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<IsUserInChatRequest, bool>
{
    public async Task<bool> Handle(IsUserInChatRequest request, CancellationToken cancellationToken = default)
    {
        var chat = await unitOfWork.ChatRepository.GetByIdAsync(request.chatId, cancellationToken, c => c.Users);
        var user = chat.Users.FirstOrDefault(u => u.Id == request.userId);
        if (user is null)
        {
            return false;
        }
        return true;
    }
}