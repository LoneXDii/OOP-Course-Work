namespace Server.Application.ChatUseCases.Commands;

public sealed record AddChatRequest(Chat chat) : IRequest<Chat> { }

internal class AddChatRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<AddChatRequest, Chat>
{
    public async Task<Chat> Handle(AddChatRequest request, CancellationToken cancellationToken = default)
    {
        await unitOfWork.ChatRepository.AddAsync(request.chat, cancellationToken);
        await unitOfWork.SaveAllAsync();
        return request.chat;
    }
}
