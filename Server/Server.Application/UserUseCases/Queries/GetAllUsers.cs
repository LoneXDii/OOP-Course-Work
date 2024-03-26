namespace Server.Application.UserUseCases.Commands;

public sealed record GetAllUsersRequest() : IRequest<IEnumerable<User>> { }

internal class GetAllUsersRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllUsersRequest, IEnumerable<User>>
{
    public async Task<IEnumerable<User>> Handle(GetAllUsersRequest request, CancellationToken cancellationToken)
    {
        return await unitOfWork.UserRepository.ListAllAsync(cancellationToken);
    }
}

