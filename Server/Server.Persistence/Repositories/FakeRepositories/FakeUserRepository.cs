using System.Linq.Expressions;

namespace Server.Persistence.Repositories.FakeRepositories;

internal class FakeUserRepository : IRepository<User>
{
    private List<User> _users = new();

    public FakeUserRepository()
    {
        for(int i = 0; i < 10; i++)
        {
            var user = new User($"User {i}", $"User{i}", $"password{i}");
            user.AuthorizationToken = $"auth{1}";
            user.Id = i + 1;
            _users.Add(user);
        }
    }

    public async Task<User?> GetByIdAsync(int id,
                         CancellationToken cancellationToken = default,
                         params Expression<Func<User, object>>[] includedProperties)
    {
        return await Task.Run(() => _users.FirstOrDefault(u => u.Id == id));
    }

    public async Task<IReadOnlyList<User>> ListAllAsync(CancellationToken cancellationToken = default)
    {
        return await Task.Run(() => _users);
    }

    public async Task<IReadOnlyList<User>> ListAsync(Expression<Func<User, bool>> filter,
                                     CancellationToken cancellationToken = default,
                                     params Expression<Func<User, object>>[] includedProperties)
    {
        var data = _users.AsQueryable();
        return await Task.Run(() => data.Where(filter).ToList() as IReadOnlyList<User>);
    }

    public async Task AddAsync(User entity, CancellationToken cancellationToken = default)
    {
        await Task.Run(() => _users.Add(entity));
    }

    public async Task UpdateAsync(User entity, CancellationToken cancellationToken = default)
    {
        await FirstOrDefaultAsync(u => u.Id == entity.Id);
    }

    public async Task DeleteAsync(User entity, CancellationToken cancellationToken = default)
    {
        await Task.Run(() => _users.Remove(entity));
    }

    public async Task<User?> FirstOrDefaultAsync(Expression<Func<User, bool>> filter,
                                 CancellationToken cancellationToken = default)
    {
        return await Task.Run(() => _users.FirstOrDefault(filter.Compile()));
    }
}
