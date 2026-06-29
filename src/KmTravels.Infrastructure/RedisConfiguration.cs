using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace SnlEngineering.Infrastructure;

internal static class RedisConfiguration
{
    public static string? ResolveEndpoint(IConfiguration configuration, ILogger logger)
    {
        if (!configuration.GetValue("Redis:Enabled", defaultValue: true))
        {
            logger.LogInformation("Redis:Enabled is false. Using in-memory cache.");
            return null;
        }

        var configured = configuration["Redis:Configuration"];
        if (string.IsNullOrWhiteSpace(configured))
        {
            logger.LogInformation("Redis:Configuration not set. Using in-memory cache.");
            return null;
        }

        var trimmed = configured.Trim();
        if (LooksLikeSqlConnectionString(trimmed))
        {
            logger.LogWarning(
                "Redis:Configuration is invalid (SQL-style string). Using in-memory cache. " +
                "Set Redis:Configuration to host:port, e.g. 154.61.75.112:6379");
            return null;
        }

        return trimmed;
    }

    public static IConnectionMultiplexer? TryConnect(string? endpoint, ILogger logger)
    {
        if (string.IsNullOrWhiteSpace(endpoint))
            return null;

        IConnectionMultiplexer? multiplexer = null;
        try
        {
            var options = ConfigurationOptions.Parse(endpoint);
            options.AbortOnConnectFail = true;
            options.ConnectTimeout = 3000;
            options.SyncTimeout = 3000;

            multiplexer = ConnectionMultiplexer.Connect(options);
            if (!multiplexer.IsConnected)
            {
                multiplexer.Dispose();
                logger.LogWarning("Redis at {Endpoint} did not connect. Using in-memory cache.", endpoint);
                return null;
            }

            logger.LogInformation("Connected to Redis at {Endpoint}.", endpoint);
            return multiplexer;
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Redis unavailable at {Endpoint}. Using in-memory cache.", endpoint);
            multiplexer?.Dispose();
            return null;
        }
    }

    private static bool LooksLikeSqlConnectionString(string value) =>
        value.Contains("Server=", StringComparison.OrdinalIgnoreCase)
        || value.Contains("Database=", StringComparison.OrdinalIgnoreCase)
        || value.Contains("User Id=", StringComparison.OrdinalIgnoreCase)
        || value.Contains("Password=", StringComparison.OrdinalIgnoreCase);
}
