namespace Server.Application.MessageUseCases.Commands;

public sealed record DeleteMessageRequest(int messageId) : IRequest { }

internal class DeleteMessageRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteMessageRequest>
{
    public async Task Handle(DeleteMessageRequest request, CancellationToken cancellationToken = default)
    {
        var message = await unitOfWork.MessageRepository.FirstOrDefaultAsync(m => m.Id == request.messageId);

        if (message is null) return;

        await unitOfWork.MessageRepository.DeleteAsync(message, cancellationToken);
        await unitOfWork.SaveAllAsync();
    }
}

