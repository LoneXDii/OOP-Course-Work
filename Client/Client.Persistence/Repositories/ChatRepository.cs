using Client.Domain.Abstractions;
using Client.Domain.Entitites;
using Client.Persistence.Services;
using System.Linq.Expressions;

namespace Client.Persistence.Repositories;

internal class ChatRepository : IRepository<Chat>
{
    private readonly IServerService _serverService;
    protected List<Chat> _entities = new();

    public ChatRepository(IServerService serverService)
    {
        _serverService = serverService;

    }

    public Task<Chat?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<Chat>> ListAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<Chat>> ListAsync(Expression<Func<Chat, bool>> filter, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(Chat entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Chat entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Chat entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Chat?> FirstOrDefaultAsync(Expression<Func<Chat, bool>> filter, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
