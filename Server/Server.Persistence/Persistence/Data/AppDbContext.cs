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
        modelBuilder.Entity<Message>()
                    .HasOne(m => m.User)
                    .WithMany()
                    .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<Message>()
                    .HasOne(m => m.Chat)
                    .WithMany()
                    .OnDelete(DeleteBehavior.SetNull);
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<Message> Messages { get; set; }
}
