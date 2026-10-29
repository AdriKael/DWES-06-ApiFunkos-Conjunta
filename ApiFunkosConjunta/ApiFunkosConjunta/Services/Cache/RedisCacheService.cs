namespace ApiFunkosConjunta.Services.Cache;

using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

/// <summary>
/// Implementación de caché usando IDistributedCache (Redis en prod, memoria en dev).
/// </summary>
public class RedisCacheService(
    IDistributedCache cache,
    ILogger<RedisCacheService> logger
) : ICacheService
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<T?> GetAsync<T>(string key)
    {
        try
        {
            var cached = await cache.GetStringAsync(key);
            if (string.IsNullOrEmpty(cached))
            {
                logger.LogDebug("Cache miss para clave: {Key}", key);
                return default;
            }
            logger.LogDebug("Cache hit para clave: {Key}", key);
            return JsonSerializer.Deserialize<T>(cached, _jsonOptions);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error al obtener caché para clave: {Key}", key);
            return default;
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        try
        {
            var json = JsonSerializer.Serialize(value, _jsonOptions);
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(5)
            };
            await cache.SetStringAsync(key, json, options);
            logger.LogDebug("Valor cacheado para clave: {Key}", key);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error al guardar en caché para clave: {Key}", key);
        }
    }

    public async Task RemoveAsync(string key)
    {
        try
        {
            await cache.RemoveAsync(key);
            logger.LogDebug("Entrada de caché eliminada para clave: {Key}", key);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error al eliminar caché para clave: {Key}", key);
        }
    }
}
