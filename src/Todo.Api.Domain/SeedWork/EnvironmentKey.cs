using Microsoft.Extensions.Configuration;

namespace Todo.Api.Domain.SeedWork
{
    public class EnvironmentKey() : ObjectValidator
    {
        public Postgres PostgresInformation { get; } = new();
        public App AppInformation { get; } = new();
        public static Type TypeInformation { get; set; } = GetEnvironment();
        public Redis RedisInformation { get; } = new();

        public sealed class Postgres
        {
            public string Server { get; set; } = string.Empty;
            public string UserId { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public string DataBase { get; set; } = string.Empty;

            public string ConnectionString
            {
                get
                {
                    return $"Host={Server};Port=5432;Username={UserId};Password={Password};Database={DataBase}";
                }
            }

        }

        public enum Type
        {
            DEV,
            QA,
            PROD
        }

        public sealed class App
        {
            public string HeaderKey { get; set; } = string.Empty;
        }

        public sealed class Redis
        {
            public string Server { get; set; } = string.Empty;
            public string User { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public int CacheExpirationHours { get; set; } = default;

            public string ConnectionString
            {
                get
                {
                    return $"{Server}:6379";
                }
            }
        }
        public bool IsValid() => AllPropertiesAreFilled(this);

        private static Type GetEnvironment()
        {
            string env = Environment.GetEnvironmentVariable(Constant.APP_ENV)!;
            return env switch
            {
                Constant.APP_ENV_QA => Type.QA,
                Constant.APP_ENV_PROD => Type.PROD,
                _ => Type.DEV,
            };
        }

        public static T GetVariable<T>(string? constant, IConfiguration configuration)
        {
            try
            {
                if (constant is null) throw new ArgumentNullException(constant);

                if (TypeInformation == Type.DEV)
                {
                    return (T)Convert.ChangeType(configuration[constant]! ?? string.Empty, typeof(T));
                }

                return (T)Convert.ChangeType(Environment.GetEnvironmentVariable(constant)! ?? string.Empty, typeof(T));
            }
            catch (Exception)
            {
                throw new NotImplementedException($"Missing an environment variable: {constant}");
            }
        }

        public static T GetVariable<T>(string constant, IConfiguration configuration, Dictionary<string, string> secrets)
        {
            try
            {
                if (TypeInformation != Type.DEV)
                {
                    return (T)Convert.ChangeType(secrets.GetValueOrDefault(constant) ?? string.Empty, typeof(T));
                }
                else
                {
                    var envKey = constant.Replace("-", "_").ToUpperInvariant();
                    return (T)Convert.ChangeType(configuration[envKey] ?? string.Empty, typeof(T));
                }
            }
            catch (Exception)
            {
                throw new NotImplementedException($"Missing an environment variable: {constant}");
            }
        }
    }
}
