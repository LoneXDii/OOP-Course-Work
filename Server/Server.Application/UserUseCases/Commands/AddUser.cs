namespace Server.Application.UserUseCases.Commands;

public sealed record AddUserRequest(User user) : IRequest<User> { }

internal class AddUserRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<AddUserRequest, User>
{
    public async Task<User> Handle(AddUserRequest request, CancellationToken cancellationToken = default)
    {
        var user = await unitOfWork.UserRepository.FirstOrDefaultAsync(u => u.Login == request.user.Login, cancellationToken);
        if (user is null)
        {
            await unitOfWork.UserRepository.AddAsync(request.user, cancellationToken);
            await unitOfWork.SaveAllAsync();
            return request.user;
        }
        throw new Exception("There is user with such login");
    }
}
