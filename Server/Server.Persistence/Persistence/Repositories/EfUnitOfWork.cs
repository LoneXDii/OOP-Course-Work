using Microsoft.EntityFrameworkCore;
using Server.Infrastructure.Persistence.Data;

namespace Server.Infrastructure.Persistence.Repositories;

internal class EfUnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;
    private readonly Lazy<IRepository<User>> _users;
    private readonly Lazy<IRepository<Chat>> _chats;
    private readonly Lazy<IRepository<Message>> _messages;

    public EfUnitOfWork(AppDbContext context)
    {
        _dbContext = context;
        _users = new Lazy<IRepository<User>>(() => new EfRepository<User>(context));
        _chats = new Lazy<IRepository<Chat>>(() => new EfRepository<Chat>(context));
        _messages = new Lazy<IRepository<Message>>(() => new EfRepository<Message>(context));
    }

    public IRepository<User> UserRepository => _users.Value;
    public IRepository<Chat> ChatRepository => _chats.Value;
    public IRepository<Message> MessageRepository => _messages.Value;

    public async Task CreateDataBaseAsync() => await _dbContext.Database.EnsureCreatedAsync();
    public async Task DeleteDataBaseAsync() => await _dbContext.Database.EnsureDeletedAsync();
    public async Task SaveAllAsync() => await _dbContext.SaveChangesAsync();
}
