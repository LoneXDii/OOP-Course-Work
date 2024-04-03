﻿using System.Linq.Expressions;

namespace Server.Infrastructure.Persistence.Repositories.FakeRepositories;

internal class FakeChatRepository : IRepository<Chat>
{
    private List<Chat> _chats = new();

    public FakeChatRepository()
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
            _chats.Add(chat);
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
        entity.Id = _chats[_chats.Count - 1].Id + 1;
        await Task.Run(() => _chats.Add(entity));
    }

    public async Task UpdateAsync(Chat entity, CancellationToken cancellationToken = default)
    {
        var chat = await FirstOrDefaultAsync(c => c.Id == entity.Id);
        if (chat is null) return;
        chat.Name = entity.Name;
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