namespace Server.Application.UserUseCases.Queries;

public sealed record GetUserByIdRequest(int userId) : IRequest<User?> { }

internal class GetUserByIdRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetUserByIdRequest, User?>
{
    public async Task<User?> Handle(GetUserByIdRequest request, CancellationToken cancellationToken = default)
    {
        return await unitOfWork.UserRepository.FirstOrDefaultAsync(u => u.Id == request.userId, cancellationToken);
    }
}
