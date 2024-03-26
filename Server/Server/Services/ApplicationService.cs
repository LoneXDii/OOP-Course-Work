
using MediatR;
using Server.Application.UserUseCases.Commands;

namespace Server.Services;


internal interface IApplicationService
{
    void Run();
}

internal class ApplicationService : IApplicationService
{
    private readonly IMediator _mediator;

    public ApplicationService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async void Run()
    {
        var a = await _mediator.Send(new GetAllUsersRequest());
        var b = 1;
    }

}
