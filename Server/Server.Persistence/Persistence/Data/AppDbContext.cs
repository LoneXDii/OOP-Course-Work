using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Migrations;

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
                    .HasForeignKey(c => c.CreatorId)
                    .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Message>()
                    .HasOne<User>()
                    .WithMany()
                    .HasForeignKey(m => m.SenderId)
                    .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<Message>()
                    .HasOne<Chat>()
                    .WithMany()
                    .HasForeignKey(m => m.ChatId)
                    .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<ChatMember>()
                    .HasOne<User>()
                    .WithMany()
                    .HasForeignKey(m => m.UserId)
                    .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<ChatMember>()
                    .HasOne<Chat>()
                    .WithMany()
                    .HasForeignKey(m => m.ChatId)
                    .OnDelete(DeleteBehavior.SetNull);
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<ChatMember> ChatMembers { get; set; }
}
