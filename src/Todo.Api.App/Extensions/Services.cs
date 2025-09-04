using Microsoft.AspNetCore.Http.Json;
using System.Text.Json.Serialization;
using Todo.Api.App.Factory;
using Todo.Api.Domain.Extensions;
using Todo.Api.Infra.Extensions;

namespace Todo.Api.App.Extensions
{
    public static class ServicesExtensions
    {
        private static void AddApp(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(ConnectionMultiplexerFactory.Create);

            serviceCollection.Configure<JsonOptions>(options =>
            {
                options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            serviceCollection.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.PropertyNameCaseInsensitive = true;
                options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        }
        public static void AddCustomServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddDomain();
            serviceCollection.AddInfra();
            serviceCollection.AddApp();
        }
    }
}
