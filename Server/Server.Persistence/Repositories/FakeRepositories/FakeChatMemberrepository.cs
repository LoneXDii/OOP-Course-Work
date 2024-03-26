using Server.Domain.Entities;
using System.Linq.Expressions;

namespace Server.Persistence.Repositories.FakeRepositories;

internal class FakeChatMemberRepository : IRepository<ChatMember>
{
    private List<ChatMember> _members = new();

    public FakeChatMemberRepository()
    {
        for (int i = 0; i < 10; i++)
        {
            ChatMember member = new ChatMember(0, 0);
            if(i < 5)
            {
                member = new ChatMember(i + 1, 1);
            }
            else
            {
                member = new ChatMember(i + 1, 2);
            }
            member.Id = i + 1;
            _members.Add(member);
        }
    }

    public async Task<ChatMember?> GetByIdAsync(int id,
                         CancellationToken cancellationToken = default,
                         params Expression<Func<ChatMember, object>>[] includedProperties)
    {
        return await Task.Run(() => _members.FirstOrDefault(u => u.Id == id));
    }

    public async Task<IReadOnlyList<ChatMember>> ListAllAsync(CancellationToken cancellationToken = default)
    {
        return await Task.Run(() => _members);
    }

    public async Task<IReadOnlyList<ChatMember>> ListAsync(Expression<Func<ChatMember, bool>> filter,
                                     CancellationToken cancellationToken = default,
                                     params Expression<Func<ChatMember, object>>[] includedProperties)
    {
        var data = _members.AsQueryable();
        return await Task.Run(() => data.Where(filter).ToList() as IReadOnlyList<ChatMember>);
    }

    public async Task AddAsync(ChatMember entity, CancellationToken cancellationToken = default)
    {
        entity.Id = _members.Count + 1;
        await Task.Run(() => _members.Add(entity));
    }

    public async Task UpdateAsync(ChatMember entity, CancellationToken cancellationToken = default)
    {
        await FirstOrDefaultAsync(u => u.Id == entity.Id);
    }

    public async Task DeleteAsync(ChatMember entity, CancellationToken cancellationToken = default)
    {
        await Task.Run(() => _members.Remove(entity));
    }

    public async Task<ChatMember?> FirstOrDefaultAsync(Expression<Func<ChatMember, bool>> filter,
                                 CancellationToken cancellationToken = default)
    {
        return await Task.Run(() => _members.FirstOrDefault(filter.Compile()));
    }
}
