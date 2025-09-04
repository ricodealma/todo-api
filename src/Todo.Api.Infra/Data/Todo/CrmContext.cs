using Microsoft.EntityFrameworkCore;
using Todo.Api.Domain.SeedWork;
using Todo.Api.Infra.Data.Todo.Entities;

namespace Todo.Api.Infra.Data.Todo
{
    public class TodoContext(DbContextOptions<TodoContext> options, EnvironmentKey environmentKey, bool test = false) : DbContext(options), ITodoContext
    {
        private readonly EnvironmentKey _environmentKey = environmentKey;
        public DbSet<TodoDto> Todo { get; set; }

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
            modelBuilder.Entity<TodoDto>()
                .Property(o => o.Id)
                .ValueGeneratedNever();
        }
    }
}
