using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Notes.Models;

public class DatabaseContext : DbContext
{
   public DbSet<UserModel> Users { get; set; }
   
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserModel>().HasKey(u => u.Id).HasName("PK_id");
    }
}