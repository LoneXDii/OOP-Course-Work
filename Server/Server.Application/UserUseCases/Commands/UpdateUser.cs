namespace Server.Application.UserUseCases.Commands;

public sealed record UpdateUserRequest(User user) : IRequest<User> { }

internal class UpdateUserRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateUserRequest, User>
{
    public async Task<User> Handle(UpdateUserRequest request, CancellationToken cancellationToken = default)
    {
        await unitOfWork.UserRepository.UpdateAsync(request.user, cancellationToken);
        await unitOfWork.SaveAllAsync();
        return request.user;
    }
}
