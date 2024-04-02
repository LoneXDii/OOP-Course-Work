namespace Server.Application.UserUseCases.Commands;

public sealed record AddUserRequest(User user) : IRequest<User> { }

internal class AddUserRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<AddUserRequest, User>
{
    public async Task<User> Handle(AddUserRequest request, CancellationToken cancellationToken = default)
    {
        await unitOfWork.UserRepository.AddAsync(request.user, cancellationToken);
        await unitOfWork.SaveAllAsync();
        return request.user;
    }
}
