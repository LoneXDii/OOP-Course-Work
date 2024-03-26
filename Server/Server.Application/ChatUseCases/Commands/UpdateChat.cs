namespace Server.Application.ChatUseCases.Commands;

public sealed record UpdateChatRequest(Chat chat) : IRequest<Chat> { }

internal class UpdateChatRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateChatRequest, Chat>
{
    public async Task<Chat> Handle(UpdateChatRequest request, CancellationToken cancellationToken)
    {
        await unitOfWork.ChatRepository.UpdateAsync(request.chat, cancellationToken);
        await unitOfWork.SaveAllAsync();
        return request.chat;
    }
}
