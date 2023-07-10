using CapstoneProjectToDoApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace CapstoneProjectToDoApplication.Database.DbConnection
{
    public class ToDoDbContext : DbContext
    {
        public ToDoDbContext(DbContextOptions<ToDoDbContext> options)
        : base(options)
        {
        }

        private readonly string _connectionString = "Server=(localdb)\\mssqllocaldb;Database=DatabaseToDoApplication;Trusted_Connection=True;";

        public DbSet<ToDoList> ToDoLists { get; set; }
        public DbSet<ToDoTask> ToDoTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ToDoList>()
                .HasMany(x => x.Tasks)
                .WithOne(x => x.List)
                .HasForeignKey(x => x.ListId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
