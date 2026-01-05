using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbContextOptions<AppDbContext> _options { get; } = options;

    public DbSet<Project> Projects => Set<Project>();
    public DbSet<ProjectTask> Tasks => Set<ProjectTask>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Project
        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.HasIndex(x => x.Name)
                  .IsUnique();

            entity.Property(x => x.CreatedAt)
                  .IsRequired();
        });

        // Task
        modelBuilder.Entity<ProjectTask>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Title)
                  .IsRequired()
                  .HasMaxLength(200);

            entity.Property(x => x.Status)
                  .IsRequired();

            entity.HasOne<Project>()
                  .WithMany()
                  .HasForeignKey(x => x.ProjectId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(x => x.Email)
                  .IsRequired()
                  .HasMaxLength(200);
        });

    }
}