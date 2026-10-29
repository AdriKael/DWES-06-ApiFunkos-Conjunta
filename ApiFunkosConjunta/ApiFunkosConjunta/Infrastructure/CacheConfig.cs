namespace ApiFunkosConjunta.Infrastructure;

using Microsoft.Extensions.DependencyInjection;
using Serilog;
using ApiFunkosConjunta.Services.Cache;

public static class CacheConfig
{
    public static IServiceCollection AddCache(this IServiceCollection services, IWebHostEnvironment environment, IConfiguration configuration)
    {
        if (environment.IsDevelopment())
        {
            Log.Information("Configurando caché en memoria (desarrollo local)...");
            services.AddDistributedMemoryCache();
        }
        else
        {
            var redisConfig = configuration["Cache:RedisConfiguration"];
            if (string.IsNullOrEmpty(redisConfig))
                throw new InvalidOperationException("Cache:RedisConfiguration no está definida.");
            Log.Information("Configurando caché Redis (producción)...");
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConfig;
                options.InstanceName = configuration["Cache:RedisInstanceName"] ?? "Funkos:";
            });
        }
        services.AddSingleton<ICacheService, RedisCacheService>();
        return services;
    }
}
