using Microsoft.EntityFrameworkCore;
using KanbanBoard.DAL.EfDbContext.DTO;

namespace KanbanBoard.DAL.EfDbContext
{
    public class KanbanBoardDbContext : DbContext
    {
        public KanbanBoardDbContext(DbContextOptions<KanbanBoardDbContext> options) : base(options)
        {
        }

        public DbSet<DbColumn> Columns { get; set; } = default!;
        public DbSet<DbComment> Comments { get; set; } = default!;
        public DbSet<DbProject> Projects { get; set; } = default!;
        public DbSet<DbTable> Tables { get; set; } = default!;
        public DbSet<DbTask> Tasks { get; set; } = default!;
        public DbSet<DbUser> Users { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // define tables
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
                .ToTable("Users")
                .HasMany(u => u.Tasks)
                .WithMany(t => t.Users);
            modelBuilder.Entity<DbUser>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<DbUser>()
                .HasMany(u => u.Projects)
                .WithMany(p => p.Users);
            modelBuilder.Entity<DbUser>()
                .HasMany(u => u.Comments)
                .WithOne(c => c.Author);

            // define seeded data
            var Project1 = new DbProject { Id = 1, Name = "Project 1" };
            modelBuilder.Entity<DbProject>().HasData(Project1);
            modelBuilder.Entity<DbProject>().HasData(new DbProject { Id = 2, Name = "Project 2" });

            var Table1 = new { Id = 2, Name = "Table 1", ProjectId = Project1.Id };
            modelBuilder.Entity<DbTable>().HasData(Table1);

            var Col1 = new { Id = 3, Name = "Col 1", Number = 0, TableId = Table1.Id };
            modelBuilder.Entity<DbColumn>().HasData(Col1);
            modelBuilder.Entity<DbColumn>().HasData(new { Id = 4, Name = "Col 2", Number = 1, TableId = Table1.Id });

            var Task1 = new { Id = 5, ColumnId = Col1.Id, Name = "Task 1", Number = 0, Description = "Task 1: description..." };
            var Task2 = new { Id = 6, ColumnId = Col1.Id, Name = "Task 2", Number = 1, Description = "Task 2: description..." };
            var Task3 = new { Id = 7, ColumnId = Col1.Id, Name = "Task 3", Number = 2, Description = "Task 3: description..." };
            modelBuilder.Entity<DbTask>().HasData(Task1);
            modelBuilder.Entity<DbTask>().HasData(Task2);
            modelBuilder.Entity<DbTask>().HasData(Task3);
        }
    }
}