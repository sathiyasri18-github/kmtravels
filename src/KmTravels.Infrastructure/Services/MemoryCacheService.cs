using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using KmTravels.Core.Interfaces;

namespace KmTravels.Infrastructure.Services;

public class MemoryCacheService : ICacheService
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<MemoryCacheService> _logger;
    private static readonly TimeSpan DefaultExpiry = TimeSpan.FromMinutes(30);
    private readonly ConcurrentDictionary<string, byte> _prefixKeys = new();

    public MemoryCacheService(IMemoryCache cache, ILogger<MemoryCacheService> logger)
    {
        _cache = cache;
        _logger = logger;
        _logger.LogInformation("Using in-memory cache (Redis unavailable or not configured).");
    }

    public Task<T?> GetAsync<T>(string key) =>
        Task.FromResult(_cache.TryGetValue(key, out T? value) ? value : default);

    public Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        _cache.Set(key, value, expiry ?? DefaultExpiry);
        _prefixKeys.TryAdd(key, 0);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key)
    {
        _cache.Remove(key);
        _prefixKeys.TryRemove(key, out _);
        return Task.CompletedTask;
    }

    public Task RemoveByPrefixAsync(string prefix)
    {
        foreach (var key in _prefixKeys.Keys.Where(k => k.StartsWith(prefix, StringComparison.Ordinal)))
        {
            _cache.Remove(key);
            _prefixKeys.TryRemove(key, out _);
        }
        return Task.CompletedTask;
    }
}
