namespace Todo.Api.App.Factory
{
    public static class ConnectionMultiplexerFactory
    {
        public static IConnectionMultiplexer? Create(IServiceProvider serviceProvider)
        {
            try
            {
                var environmentKey = serviceProvider.GetRequiredService<EnvironmentKey>();
                ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(environmentKey.RedisInformation.ConnectionString);
                return connection;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error creating Redis connection: {ex.Message}");
                return null;
            }
        }
    }
}
