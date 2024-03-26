namespace Server.Application.ChatUseCases.Commands;

public sealed record AddChatRequest(Chat chat) : IRequest<Chat> { }

internal class AddChatRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<AddChatRequest, Chat>
{
    public async Task<Chat> Handle(AddChatRequest request, CancellationToken cancellationToken)
    {
        await unitOfWork.ChatRepository.AddAsync(request.chat, cancellationToken);
        await unitOfWork.ChatMemberRepository.AddAsync(new ChatMember(request.chat.CreatorId, request.chat.Id), cancellationToken);
        await unitOfWork.SaveAllAsync();
        return request.chat;
    }
}
