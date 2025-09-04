using Microsoft.Extensions.DependencyInjection;
using Todo.Api.Domain.Aggregates.Todo;
using Todo.Api.Infra.Data.Todo;
using Todo.Api.Infra.External;
using Todo.Api.Infra.Repositories;

namespace Todo.Api.Infra.Extensions
{
    public static class InfraServicesExtensions
    {
        private static void AddDaos(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<ITodoDao, TodoDao>();
            serviceCollection.AddScoped<IAwsDao, AwsDao>();
        }

        private static void AddRepositories(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<ITodoRepository, TodoRepository>();
            serviceCollection.AddScoped<IAwsRepository, AwsRepository>();
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
