using Microsoft.Extensions.DependencyInjection;
using Todo.Api.Domain.Aggregates.Todo;
using Todo.Api.Infra.Data.Todo;
using Todo.Api.Infra.Data.Todo.Entities;
using Todo.Api.Infra.External;
using Todo.Api.Infra.Repositories;

namespace Todo.Api.Infra.Extensions
{
    public static class InfraServicesExtensions
    {
        private static void AddDaos(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<ITodoDao, TodoDao>();
        }

        private static void AddRepositories(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<ITodoRepository, TodoRepository>();
        }

        private static void AddPersistence(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IDistributedMemoryCacheDao, DistributedMemoryCacheDao>();
            serviceCollection.AddDbContext<ITodoContext, TodoContext>();
        }



        public static void AddInfra(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDaos();
            serviceCollection.AddRepositories();
            serviceCollection.AddPersistence();
        }
    }
}
