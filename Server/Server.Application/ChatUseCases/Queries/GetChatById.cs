namespace Server.Application.ChatUseCases.Queries;

public sealed record GetChatByIdRequest(int chatId) : IRequest<Chat?> { }

internal class GetChatByIdRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetChatByIdRequest, Chat?>
{
    public async Task<Chat?> Handle(GetChatByIdRequest request, CancellationToken cancellationToken = default)
    {
        return await unitOfWork.ChatRepository.FirstOrDefaultAsync(c => c.Id == request.chatId, cancellationToken);
    }
}
