namespace Server.Application.ChatUseCases.Commands;

public sealed record DeleteUserFromChatRequest(int userId, int chatId) : IRequest { }

internal class DeleteUserFromChatRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteUserFromChatRequest>
{
    public async Task Handle(DeleteUserFromChatRequest request, CancellationToken cancellationToken = default)
    {
        var user = await unitOfWork.UserRepository.GetByIdAsync(request.userId, cancellationToken);
        var chat = await unitOfWork.ChatRepository.GetByIdAsync(request.chatId, cancellationToken, c => c.Users);
        if (user is null || chat is null) return;

        chat.Users.Remove(user);
        await unitOfWork.SaveAllAsync();
    }
}
