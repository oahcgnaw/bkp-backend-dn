using bkpDN.Models;
using Microsoft.EntityFrameworkCore;

namespace bkpDN.Data;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {}
    
    public DbSet<ValidationCode> ValidationCodes { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Account> Accounts { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasMany(u => u.Tags)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.User_id);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Accounts)
            .WithOne(a => a.User)
            .HasForeignKey(a => a.User_id);

        modelBuilder.Entity<Tag>()
            .HasMany(t => t.Accounts)
            .WithOne(a => a.Tag)
            .HasForeignKey(a => a.Tag_id);
    }
    
}
    
