using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Server.Persistence.Repositories.FakeRepositories;

internal class FakeChatrepository : IRepository<Chat>
{
    private List<Chat> _chats = new();

    public FakeChatrepository()
    {
        for (int i = 0; i < 2; i++)
        {
            var chat = new Chat($"Chat {i}");
            if (i == 0)
            {
                chat.CreatorId = Random.Shared.Next(1, 5);
            }
            else
            {
                chat.CreatorId = Random.Shared.Next(6, 10);
            }
            chat.Id = i + 1;
        }
    }

    public async Task<Chat?> GetByIdAsync(int id,
                         CancellationToken cancellationToken = default,
                         params Expression<Func<Chat, object>>[] includedProperties)
    {
        return await Task.Run(() => _chats.FirstOrDefault(u => u.Id == id));
    }

    public async Task<IReadOnlyList<Chat>> ListAllAsync(CancellationToken cancellationToken = default)
    {
        return await Task.Run(() => _chats);
    }

    public async Task<IReadOnlyList<Chat>> ListAsync(Expression<Func<Chat, bool>> filter,
                                     CancellationToken cancellationToken = default,
                                     params Expression<Func<Chat, object>>[] includedProperties)
    {
        var data = _chats.AsQueryable();
        return await Task.Run(() => data.Where(filter).ToList() as IReadOnlyList<Chat>);
    }

    public async Task AddAsync(Chat entity, CancellationToken cancellationToken = default)
    {
        await Task.Run(() => _chats.Add(entity));
    }

    public async Task UpdateAsync(Chat entity, CancellationToken cancellationToken = default)
    {
        await FirstOrDefaultAsync(u => u.Id == entity.Id);
    }

    public async Task DeleteAsync(Chat entity, CancellationToken cancellationToken = default)
    {
        await Task.Run(() => _chats.Remove(entity));
    }

    public async Task<Chat?> FirstOrDefaultAsync(Expression<Func<Chat, bool>> filter,
                                 CancellationToken cancellationToken = default)
    {
        return await Task.Run(() => _chats.FirstOrDefault(filter.Compile()));
    }
}
