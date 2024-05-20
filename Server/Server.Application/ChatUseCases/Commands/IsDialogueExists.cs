namespace Server.Application.ChatUseCases.Commands;

public sealed record IsDialogueExistsRequest(int user1Id, int user2Id) : IRequest<Chat?> { }

internal class IsDialogueExistsRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<IsDialogueExistsRequest, Chat?>
{
    public async Task<Chat?> Handle(IsDialogueExistsRequest request, CancellationToken cancellationToken = default)
    {
        var dialogues = await unitOfWork.ChatRepository.ListAsync(d => d.IsDialogue, cancellationToken,
                                                            d => d.Users);
        if (dialogues is not null)
        {
            var dialogue = dialogues.FirstOrDefault(c => (c.Users[0].Id == request.user1Id && c.Users[1].Id == request.user2Id)
                                                      || (c.Users[0].Id == request.user2Id && c.Users[1].Id == request.user1Id));
            if (dialogue is not null)
            {
                return dialogue;
            }
        }
        return null;
    }
}