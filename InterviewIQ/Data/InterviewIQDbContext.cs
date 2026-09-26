using InterviewIQ.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace InterviewIQ.Data;

public class InterviewIQDbContext : DbContext
{
    public InterviewIQDbContext(
        DbContextOptions<InterviewIQDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("IQ_Users");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.FullName)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(255);

            entity.HasIndex(x => x.Email)
                .IsUnique();

            entity.Property(x => x.PasswordHash)
                .IsRequired()
                .HasMaxLength(500);

            entity.HasOne(x => x.Role)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Role
        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("IQ_Roles");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasIndex(x => x.Name)
                .IsUnique();
        });

        // Refresh Token
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("IQ_RefreshTokens");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Token)
                .IsRequired()
                .HasMaxLength(500);

            entity.HasIndex(x => x.Token)
                .IsUnique();

            entity.HasOne(x => x.User)
                .WithMany(x => x.RefreshTokens)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Initial Roles
        modelBuilder.Entity<Role>().HasData(
             new Role
             {
                 Id = 1,
                 Name = "Super Admin",
                 IsActive = 1,
                 CreatedBy = 1,
                 CreatedDate = new DateTime(2026, 1, 1),
                 UpdatedBy = 1,
                 UpdatedDate = new DateTime(2026, 1, 1)
             },
             new Role
             {
                 Id = 2,
                 Name = "Admin",
                 IsActive = 1,
                 CreatedBy = 1,
                 CreatedDate = new DateTime(2026, 1, 1),
                 UpdatedBy = 1,
                 UpdatedDate = new DateTime(2026, 1, 1)
             },
             new Role
             {
                 Id = 3,
                 Name = "User",
                 IsActive = 1,
                 CreatedBy = 1,
                 CreatedDate = new DateTime(2026, 1, 1),
                 UpdatedBy = 1,
                 UpdatedDate = new DateTime(2026, 1, 1)
             }
         );
    }
}