using InterviewIQ.Models.Entities;
using InterviewIQ.Models.Interviews;
using Microsoft.EntityFrameworkCore;

namespace InterviewIQ.Data;

public class InterviewIQDbContext : DbContext
{
    public InterviewIQDbContext(
        DbContextOptions<InterviewIQDbContext> options)
        : base(options)
    {
    }

    // Auth Tables
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<PasswordResetToken> PasswordResetTokens
    => Set<PasswordResetToken>();

    // Interviews
    public DbSet<InterviewDomain> InterviewDomains => Set<InterviewDomain>();

    public DbSet<Topic> Topics => Set<Topic>();

    public DbSet<InterviewDifficulty> InterviewDifficulties => Set<InterviewDifficulty>();

    public DbSet<InterviewDuration> InterviewDurations => Set<InterviewDuration>();

    public DbSet<InterviewerPersona> InterviewerPersonas => Set<InterviewerPersona>();

    // Interviews
    public DbSet<InterviewSession> InterviewSessions => Set<InterviewSession>();

    public DbSet<InterviewSessionTopic> InterviewSessionTopics => Set<InterviewSessionTopic>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var seedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

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

        modelBuilder.Entity<PasswordResetToken>(entity =>
        {
            entity.ToTable("IQ_PasswordResetTokens");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.TokenHash)
                .IsRequired()
                .HasMaxLength(64);

            entity.HasIndex(x => x.TokenHash)
                .IsUnique();

            entity.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Domain and topic configuration
        modelBuilder.Entity<InterviewDomain>(entity =>
        {
            entity.ToTable("IQ_InterviewDomains");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.Description)
                .HasMaxLength(500);

            entity.HasIndex(x => x.Name)
                .IsUnique();
        });

        modelBuilder.Entity<Topic>(entity =>
        {
            entity.ToTable("IQ_Topics");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(x => x.Description)
                .HasMaxLength(500);

            entity.HasIndex(x => new { x.DomainId, x.Name })
                .IsUnique();

            entity.HasOne(x => x.Domain)
                .WithMany(x => x.Topics)
                .HasForeignKey(x => x.DomainId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Difficulty, duration and persona configuration
        modelBuilder.Entity<InterviewDifficulty>(entity =>
        {
            entity.ToTable("IQ_InterviewDifficulties");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasIndex(x => x.Name)
                .IsUnique();
        });

        modelBuilder.Entity<InterviewDuration>(entity =>
        {
            entity.ToTable("IQ_InterviewDurations", table =>
            table.HasCheckConstraint(
                "CK_IQ_InterviewDurations_DurationMinutes",
                "[DurationMinutes] > 0"));

            entity.HasKey(x => x.Id);

            entity.HasIndex(x => x.DurationMinutes)
                .IsUnique();
        });

        modelBuilder.Entity<InterviewerPersona>(entity =>
        {
            entity.ToTable("IQ_InterviewerPersonas");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.Description)
                .HasMaxLength(500);

            entity.HasIndex(x => x.Name)
                .IsUnique();
        });

        // Interviews

        modelBuilder.Entity<InterviewSession>(entity =>
        {
            entity.ToTable("IQ_InterviewSessions");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Status)
                .IsRequired()
                .HasMaxLength(30);

            entity.HasIndex(x => new { x.UserId, x.CreatedDate });

            entity.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Domain)
                .WithMany()
                .HasForeignKey(x => x.DomainId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Difficulty)
                .WithMany()
                .HasForeignKey(x => x.DifficultyId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Duration)
                .WithMany()
                .HasForeignKey(x => x.DurationId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Persona)
                .WithMany()
                .HasForeignKey(x => x.PersonaId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<InterviewSessionTopic>(entity =>
        {
            entity.ToTable("IQ_InterviewSessionTopics");

            entity.HasKey(x => x.Id);

            entity.HasIndex(x => new
            {
                x.InterviewSessionId,
                x.TopicId
            }).IsUnique();

            entity.HasOne(x => x.InterviewSession)
                .WithMany(x => x.SessionTopics)
                .HasForeignKey(x => x.InterviewSessionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.Topic)
                .WithMany()
                .HasForeignKey(x => x.TopicId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Seed Initial Roles
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

        // Seed Interview Domain
        modelBuilder.Entity<InterviewDomain>().HasData(
            new InterviewDomain
            {
                Id = 1,
                Name = "Software Development",
                Description = "Software engineering and development interviews.",
                IsActive = 1,
                CreatedBy = 1,
                CreatedDate = seedDate,
                UpdatedBy = 1,
                UpdatedDate = seedDate
            },
            new InterviewDomain
            {
                Id = 2,
                Name = "Legal",
                Description = "Legal practice, research and drafting interviews.",
                IsActive = 1,
                CreatedBy = 1,
                CreatedDate = seedDate,
                UpdatedBy = 1,
                UpdatedDate = seedDate
            },
            new InterviewDomain
            {
                Id = 3,
                Name = "Testing",
                Description = "Software quality assurance and testing interviews.",
                IsActive = 1,
                CreatedBy = 1,
                CreatedDate = seedDate,
                UpdatedBy = 1,
                UpdatedDate = seedDate
            }
        );

        // Seed Starter Topics
        modelBuilder.Entity<Topic>().HasData(
            new Topic { Id = 1, DomainId = 1, Name = "C#", Description = "C# language and programming.", IsActive = 1, CreatedBy = 1, CreatedDate = seedDate, UpdatedBy = 1, UpdatedDate = seedDate },
            new Topic { Id = 2, DomainId = 1, Name = ".NET", Description = ".NET and ASP.NET development.", IsActive = 1, CreatedBy = 1, CreatedDate = seedDate, UpdatedBy = 1, UpdatedDate = seedDate },
            new Topic { Id = 3, DomainId = 1, Name = "React", Description = "React frontend development.", IsActive = 1, CreatedBy = 1, CreatedDate = seedDate, UpdatedBy = 1, UpdatedDate = seedDate },
            new Topic { Id = 4, DomainId = 1, Name = "SQL Server", Description = "Relational database development.", IsActive = 1, CreatedBy = 1, CreatedDate = seedDate, UpdatedBy = 1, UpdatedDate = seedDate },
            new Topic { Id = 5, DomainId = 2, Name = "Mergers and Acquisitions", Description = "M&A legal concepts and transactions.", IsActive = 1, CreatedBy = 1, CreatedDate = seedDate, UpdatedBy = 1, UpdatedDate = seedDate },
            new Topic { Id = 6, DomainId = 2, Name = "Contract Drafting", Description = "Contract structure and drafting.", IsActive = 1, CreatedBy = 1, CreatedDate = seedDate, UpdatedBy = 1, UpdatedDate = seedDate },
            new Topic { Id = 7, DomainId = 2, Name = "Legal Research", Description = "Legal research and analysis.", IsActive = 1, CreatedBy = 1, CreatedDate = seedDate, UpdatedBy = 1, UpdatedDate = seedDate },
            new Topic { Id = 8, DomainId = 3, Name = "Manual Testing", Description = "Manual testing techniques.", IsActive = 1, CreatedBy = 1, CreatedDate = seedDate, UpdatedBy = 1, UpdatedDate = seedDate },
            new Topic { Id = 9, DomainId = 3, Name = "Automation Testing", Description = "Test automation techniques and tools.", IsActive = 1, CreatedBy = 1, CreatedDate = seedDate, UpdatedBy = 1, UpdatedDate = seedDate }
        );

        // Seed Difficulty Levels
        modelBuilder.Entity<InterviewDifficulty>().HasData(
            new InterviewDifficulty { Id = 1, Name = "Easy", SortOrder = 1, IsActive = 1, CreatedBy = 1, CreatedDate = seedDate, UpdatedBy = 1, UpdatedDate = seedDate },
            new InterviewDifficulty { Id = 2, Name = "Medium", SortOrder = 2, IsActive = 1, CreatedBy = 1, CreatedDate = seedDate, UpdatedBy = 1, UpdatedDate = seedDate },
            new InterviewDifficulty { Id = 3, Name = "Hard", SortOrder = 3, IsActive = 1, CreatedBy = 1, CreatedDate = seedDate, UpdatedBy = 1, UpdatedDate = seedDate },
            new InterviewDifficulty { Id = 4, Name = "Expert", SortOrder = 4, IsActive = 1, CreatedBy = 1, CreatedDate = seedDate, UpdatedBy = 1, UpdatedDate = seedDate },
            new InterviewDifficulty { Id = 5, Name = "Random", SortOrder = 5, IsActive = 1, CreatedBy = 1, CreatedDate = seedDate, UpdatedBy = 1, UpdatedDate = seedDate }
        );

        // Seed interview durations
        modelBuilder.Entity<InterviewDuration>().HasData(
            new InterviewDuration { Id = 1, DurationMinutes = 20, SortOrder = 1, IsActive = 1, CreatedBy = 1, CreatedDate = seedDate, UpdatedBy = 1, UpdatedDate = seedDate },
            new InterviewDuration { Id = 2, DurationMinutes = 30, SortOrder = 2, IsActive = 1, CreatedBy = 1, CreatedDate = seedDate, UpdatedBy = 1, UpdatedDate = seedDate },
            new InterviewDuration { Id = 3, DurationMinutes = 45, SortOrder = 3, IsActive = 1, CreatedBy = 1, CreatedDate = seedDate, UpdatedBy = 1, UpdatedDate = seedDate },
            new InterviewDuration { Id = 4, DurationMinutes = 60, SortOrder = 4, IsActive = 1, CreatedBy = 1, CreatedDate = seedDate, UpdatedBy = 1, UpdatedDate = seedDate }
        );

        // Seed interviewer personas
        modelBuilder.Entity<InterviewerPersona>().HasData(
            new InterviewerPersona { Id = 1, Name = "Friendly", Description = "Supportive and encouraging interviewer.", SortOrder = 1, IsActive = 1, CreatedBy = 1, CreatedDate = seedDate, UpdatedBy = 1, UpdatedDate = seedDate },
            new InterviewerPersona { Id = 2, Name = "Professional", Description = "Formal, balanced and professional interviewer.", SortOrder = 2, IsActive = 1, CreatedBy = 1, CreatedDate = seedDate, UpdatedBy = 1, UpdatedDate = seedDate },
            new InterviewerPersona { Id = 3, Name = "Aggressive", Description = "Challenging interviewer who probes answers firmly but respectfully.", SortOrder = 3, IsActive = 1, CreatedBy = 1, CreatedDate = seedDate, UpdatedBy = 1, UpdatedDate = seedDate },
            new InterviewerPersona { Id = 4, Name = "HR", Description = "Focuses on behavioral, communication and culture-fit questions.", SortOrder = 4, IsActive = 1, CreatedBy = 1, CreatedDate = seedDate, UpdatedBy = 1, UpdatedDate = seedDate },
            new InterviewerPersona { Id = 5, Name = "Technical Lead", Description = "Focuses on technical depth, implementation and engineering decisions.", SortOrder = 5, IsActive = 1, CreatedBy = 1, CreatedDate = seedDate, UpdatedBy = 1, UpdatedDate = seedDate },
            new InterviewerPersona { Id = 6, Name = "CTO Manager", Description = "Focuses on architecture, strategy, ownership and technical leadership.", SortOrder = 6, IsActive = 1, CreatedBy = 1, CreatedDate = seedDate, UpdatedBy = 1, UpdatedDate = seedDate }
        );
    }
}