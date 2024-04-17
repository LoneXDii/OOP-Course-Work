using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Server.Infrastructure.Persistence.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chat>()
                    .HasOne<User>()
                    .WithMany()
                    .HasForeignKey(c => c.CreatorId);

        modelBuilder.Entity<Message>()
                    .HasOne<User>()
                    .WithMany()
                    .HasForeignKey(m => m.SenderId);
        modelBuilder.Entity<Message>()
                    .HasOne<Chat>()
                    .WithMany()
                    .HasForeignKey(m => m.ChatId);

        modelBuilder.Entity<ChatMember>()
                    .HasOne<User>()
                    .WithMany()
                    .HasForeignKey(m => m.UserId);
        modelBuilder.Entity<ChatMember>()
                    .HasOne<Chat>()
                    .WithMany()
                    .HasForeignKey(m => m.ChatId);
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<ChatMember> ChatMembers { get; set; }
}
