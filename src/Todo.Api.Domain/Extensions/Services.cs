using Microsoft.Extensions.DependencyInjection;
using Todo.Api.Domain.Aggregates.Todo;
using Todo.Api.Domain.SeedWork;

namespace Todo.Api.Domain.Extensions
{
    public static class DomainServicesExtensions
    {
        private static void AddDomainServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<ITodoService, TodoService>();
        }

        private static void AddSeedWork(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<EnvironmentKey>();
        }

        public static void AddDomain(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDomainServices();
            serviceCollection.AddSeedWork();
        }
    }
}
