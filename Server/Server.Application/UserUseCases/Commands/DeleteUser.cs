namespace Server.Application.UserUseCases.Commands;

public sealed record DeleteUserRequest(int userId) : IRequest { }

internal class DeleteUserRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteUserRequest>
{
    public async Task Handle(DeleteUserRequest request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.UserRepository.FirstOrDefaultAsync(u => u.Id == request.userId);

        if (user is null) return;

        await unitOfWork.UserRepository.DeleteAsync(user, cancellationToken);
        
        var members = await unitOfWork.ChatMemberRepository.ListAsync(m => m.UserId == request.userId, cancellationToken);
        foreach (var member in members)
        {
            await unitOfWork.ChatMemberRepository.DeleteAsync(member, cancellationToken);
        }

        await unitOfWork.SaveAllAsync();
    }
}
