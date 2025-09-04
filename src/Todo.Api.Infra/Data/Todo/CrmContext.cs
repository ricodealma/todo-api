using Microsoft.EntityFrameworkCore;
using Todo.Api.Domain.SeedWork;

namespace Todo.Api.Infra.Data.Todo
{
    public class TodoContext(DbContextOptions<TodoContext> options, EnvironmentKey environmentKey, bool test = false) : DbContext(options), ITodoContext
    {
        private readonly EnvironmentKey _environmentKey = environmentKey;
        public DbSet<TodoDTO> Todo { get; set; }
        public DbSet<ClientDTO> Client { get; set; }
        public DbSet<StatusDTO> Status { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (test)
            {
                optionsBuilder
                    .UseInMemoryDatabase($"test_db_{Guid.NewGuid()}")
                    .EnableDetailedErrors();
                return;
            }

            var connectionString = _environmentKey.PostgresInformation.ConnectionString;

            optionsBuilder.UseNpgsql(connectionString);

            if (EnvironmentKey.TypeInformation == EnvironmentKey.Type.DEV)
            {
                optionsBuilder
                    .EnableDetailedErrors()
                    .EnableSensitiveDataLogging();
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoDTO>()
                .Property(o => o.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<TodoDTO>()
                .HasOne(o => o.Status)
                .WithMany()
                .HasForeignKey(o => o.StatusId)
                .IsRequired();

            modelBuilder.Entity<TodoDTO>()
                .HasOne(o => o.Client)
                .WithMany()
                .HasForeignKey(o => o.ClientId)
                .IsRequired();

            modelBuilder.Entity<TodoDTO>()
                .HasMany(o => o.Items)
                .WithOne()
                .HasForeignKey(i => i.TodoId)
                .IsRequired();
        }
    }
}
