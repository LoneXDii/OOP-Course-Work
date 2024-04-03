using Server.Domain.Entities;
using System.Linq.Expressions;

namespace Server.Infrastructure.Persistence.Repositories.FakeRepositories;

internal class FakeMessageRepository : IRepository<Message>
{
    private List<Message> _messages = new();

    public FakeMessageRepository()
    {
        for (int i = 0; i < 100; i++)
        {
            var message = new Message($"message {i}");
            message.SenderId = Random.Shared.Next(1, 10);
            if (message.SenderId <= 5)
            {
                message.ChatId = 1;
            }
            else
            {
                message.ChatId = 2;
            }
            message.Id = i + 1;
            _messages.Add(message);
        }
    }

    public async Task<Message?> GetByIdAsync(int id,
                         CancellationToken cancellationToken = default,
                         params Expression<Func<Message, object>>[] includedProperties)
    {
        return await Task.Run(() => _messages.FirstOrDefault(u => u.Id == id));
    }

    public async Task<IReadOnlyList<Message>> ListAllAsync(CancellationToken cancellationToken = default)
    {
        return await Task.Run(() => _messages);
    }

    public async Task<IReadOnlyList<Message>> ListAsync(Expression<Func<Message, bool>> filter,
                                     CancellationToken cancellationToken = default,
                                     params Expression<Func<Message, object>>[] includedProperties)
    {
        var data = _messages.AsQueryable();
        return await Task.Run(() => data.Where(filter).ToList() as IReadOnlyList<Message>);
    }

    public async Task AddAsync(Message entity, CancellationToken cancellationToken = default)
    {
        entity.Id = _messages[_messages.Count - 1].Id + 1;
        await Task.Run(() => _messages.Add(entity));
    }

    public async Task UpdateAsync(Message entity, CancellationToken cancellationToken = default)
    {
        var message = await FirstOrDefaultAsync(m => m.Id == entity.Id, cancellationToken);
        if (message is null) return;
        message.Text = entity.Text;
    }

    public async Task DeleteAsync(Message entity, CancellationToken cancellationToken = default)
    {
        await Task.Run(() => _messages.Remove(entity));
    }

    public async Task<Message?> FirstOrDefaultAsync(Expression<Func<Message, bool>> filter,
                                 CancellationToken cancellationToken = default)
    {
        return await Task.Run(() => _messages.FirstOrDefault(filter.Compile()));
    }
}
