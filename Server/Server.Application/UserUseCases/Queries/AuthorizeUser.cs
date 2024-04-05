namespace Server.Application.UserUseCases.Queries;

public sealed record AuthorizeUserRequest(string login, string password) : IRequest<User?> { }

internal class AuthorizeUserRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<AuthorizeUserRequest, User?>
{
    public async Task<User?> Handle(AuthorizeUserRequest request, CancellationToken cancellationToken = default)
    {
        return await unitOfWork.UserRepository.FirstOrDefaultAsync(u=> u.Login == request.login && u.Password == request.password 
                                                                   , cancellationToken);
    }
}