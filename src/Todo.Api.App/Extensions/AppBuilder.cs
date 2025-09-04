using Newtonsoft.Json;
using Todo.Api.Domain.SeedWork;

namespace Todo.Api.App.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        private static void FillEnvironmentKeys(EnvironmentKey environmentKey, IConfiguration configuration)
        {
            environmentKey.RedisInformation.CacheExpirationHours =
                EnvironmentKey.GetVariable<int>(Constant.REDIS_CACHE_ENTITY_EXPIRATION_HOURS, configuration);

            environmentKey.AppInformation.HeaderKey =
                EnvironmentKey.GetVariable<string>(Constant.AWS_SECRET_MANAGER_HEADER_TOKEN, configuration);

            environmentKey.PostgresInformation.Server =
                EnvironmentKey.GetVariable<string>(Constant.SQL_SERVER, configuration);

            environmentKey.PostgresInformation.DataBase =
                EnvironmentKey.GetVariable<string>(Constant.SQL_DATABASE, configuration);

            environmentKey.PostgresInformation.UserId =
                EnvironmentKey.GetVariable<string>(Constant.SQL_USER, configuration);

            environmentKey.PostgresInformation.Password =
                EnvironmentKey.GetVariable<string>(Constant.SQL_PASSWORD, configuration);

            environmentKey.RedisInformation.Password =
                EnvironmentKey.GetVariable<string>(Constant.REDIS_PASSWORD, configuration);

            environmentKey.RedisInformation.User =
                EnvironmentKey.GetVariable<string>(Constant.REDIS_USER, configuration);

            environmentKey.RedisInformation.Server =
                EnvironmentKey.GetVariable<string>(Constant.REDIS_SERVER, configuration);
        }



        private static void AddMiddlewares(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMiddleware<GatewayAuthenticationMiddleware>();
        }
        public static void FillEnvironmentVariables(this IApplicationBuilder applicationBuilder, IConfiguration configuration)
        {
            EnvironmentKey environmentKey = applicationBuilder.ApplicationServices.GetRequiredService<EnvironmentKey>();
            FillEnvironmentKeys(environmentKey, configuration);
            ValidateConfigurationBeforeStart(environmentKey, applicationBuilder.ApplicationServices);
            applicationBuilder.AddMiddlewares();

        }

        private static void ValidateConfigurationBeforeStart(EnvironmentKey environmentKey, IServiceProvider serviceProvider)
        {
            if (!environmentKey.IsValid())
                throw new Exception(JsonConvert.SerializeObject(new { ErrorMessage = "Some environment variables are not configured", DetailedError = environmentKey }));
        }

    }
}
