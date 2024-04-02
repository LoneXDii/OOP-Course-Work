namespace Server.Application.ChatUseCases.Commands;

public sealed record DeleteUserFromChatRequest(int userId, int chatId) : IRequest { }

internal class DeleteUserFromChatRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteUserFromChatRequest>
{
    public async Task Handle(DeleteUserFromChatRequest request, CancellationToken cancellationToken = default)
    {
        var user = await unitOfWork.UserRepository.FirstOrDefaultAsync(u => u.Id == request.userId, cancellationToken);
        var chat = await unitOfWork.ChatRepository.FirstOrDefaultAsync(c => c.Id == request.chatId, cancellationToken);
        if (user is null || chat is null) return;

        var member = await unitOfWork.ChatMemberRepository.FirstOrDefaultAsync(m => m.UserId == request.userId && m.ChatId == request.chatId
                                                                               , cancellationToken);
        if (member is null) return;

        await unitOfWork.ChatMemberRepository.DeleteAsync(member, cancellationToken);
    }
}
