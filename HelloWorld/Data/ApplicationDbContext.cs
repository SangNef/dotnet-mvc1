using HelloWorld.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HelloWorld.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Singer> Singers { get; set; }
    public DbSet<Composer> Composers { get; set; }
    public DbSet<Song> Songs { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties()
                         .Where(p => p.ClrType == typeof(string) && p.GetMaxLength() == null))
            {
                property.SetMaxLength(256);
            }
        }

        builder.Entity<Song>()
            .HasOne(s => s.Singer)
            .WithMany(a => a.Songs)
            .HasForeignKey(s => s.SingerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Song>()
            .HasOne(s => s.Composer)
            .WithMany(c => c.Songs)
            .HasForeignKey(s => s.ComposerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}