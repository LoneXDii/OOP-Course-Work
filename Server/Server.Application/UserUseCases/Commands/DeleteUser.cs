namespace Server.Application.UserUseCases.Commands;

public sealed record DeleteUserRequest(int userId) : IRequest { }

internal class DeleteUserRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteUserRequest>
{
    public async Task Handle(DeleteUserRequest request, CancellationToken cancellationToken = default)
    {
        var user = await unitOfWork.UserRepository.GetByIdAsync(request.userId, cancellationToken);

        if (user is null) return;

        await unitOfWork.UserRepository.DeleteAsync(user, cancellationToken);

        await unitOfWork.SaveAllAsync();
    }
}
