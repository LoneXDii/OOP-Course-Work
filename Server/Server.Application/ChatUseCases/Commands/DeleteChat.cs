namespace Server.Application.ChatUseCases.Commands;

public sealed record DeleteChatRequest(int chatId) : IRequest { }

internal class DeleteUserRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteChatRequest>
{
    public async Task Handle(DeleteChatRequest request, CancellationToken cancellationToken = default)
    {
        var chat = await unitOfWork.ChatRepository.FirstOrDefaultAsync(c => c.Id == request.chatId, cancellationToken);

        if (chat is null) return;

        await unitOfWork.ChatRepository.DeleteAsync(chat, cancellationToken);

        await unitOfWork.SaveAllAsync();
    }
}
