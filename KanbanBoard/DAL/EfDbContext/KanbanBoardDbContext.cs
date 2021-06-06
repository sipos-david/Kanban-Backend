using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using KanbanBoard.Models;
using System.Collections.Generic;
using KanbanBoard.DAL.EfDbContext.DTO;
using System.ComponentModel.DataAnnotations.Schema;

namespace KanbanBoard.DAL.EfDbContext
{
    public class KanbanBoardDbContext : IdentityDbContext<DbUser>
    {
        public KanbanBoardDbContext(DbContextOptions<KanbanBoardDbContext> options) : base(options)
        {
        }

        public DbSet<DbColumn> Columns { get; set; } = default!;
        public DbSet<DbComment> Comments { get; set; } = default!;
        public DbSet<DbProject> Projects { get; set; } = default!;
        public DbSet<DbTable> Tables { get; set; } = default!;
        public DbSet<DbTask> Tasks { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DbColumn>()
                .ToTable("Columns")
                .HasMany(c => c.Tasks)
                .WithOne(t => t.Column);
            modelBuilder.Entity<DbColumn>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<DbColumn>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<DbComment>()
                .ToTable("Comments")
                .HasOne(c => c.Author)
                .WithMany(u => u.Comments);
            modelBuilder.Entity<DbComment>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<DbComment>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<DbProject>()
                .ToTable("Projects")
                .HasMany(p => p.Tables)
                .WithOne(t => t.Project);
            modelBuilder.Entity<DbProject>()
                .HasMany(p => p.Users)
                .WithMany(u => u.Projects);
            modelBuilder.Entity<DbProject>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<DbProject>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<DbTable>()
                .ToTable("Tables")
                .HasKey(t => t.Id);
            modelBuilder.Entity<DbTable>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<DbTable>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<DbTask>()
                .ToTable("Tasks")
                .HasMany(t => t.Users)
                .WithMany(u => u.Tasks);
            modelBuilder.Entity<DbTask>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<DbTask>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<DbUser>()
                .ToTable("AspNetUsers", t => t.ExcludeFromMigrations());
        }
    }
}