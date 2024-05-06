using Client.Domain.Abstractions;
using Client.Domain.Entitites;
using Client.Persistence.Services;
using System.Linq.Expressions;

namespace Client.Persistence.Repositories;

internal class MessageRepository : IRepository<Message>
{
    private readonly IServerService _serverService;
    protected List<Message> _entities = new();

    public MessageRepository(IServerService serverService)
    {
        _serverService = serverService;
    }

    public Task<Message?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<Message>> ListAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<Message>> ListAsync(Expression<Func<Message, bool>> filter, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(Message entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Message entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Message entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Message?> FirstOrDefaultAsync(Expression<Func<Message, bool>> filter, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
