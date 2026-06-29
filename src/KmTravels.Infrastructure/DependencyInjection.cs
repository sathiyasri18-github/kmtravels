using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using KmTravels.Core.Interfaces;
using KmTravels.Infrastructure.Data;
using KmTravels.Infrastructure.Services;

namespace KmTravels.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddMemoryCache();
        services.AddSingleton<ICacheService>(sp =>
        {
            try
            {
                var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger<RedisCacheService>();
                var endpoint = RedisConfiguration.ResolveEndpoint(configuration, logger);
                var multiplexer = RedisConfiguration.TryConnect(endpoint, logger);

                if (multiplexer != null)
                    return new RedisCacheService(multiplexer, logger);

                return new MemoryCacheService(
                    sp.GetRequiredService<Microsoft.Extensions.Caching.Memory.IMemoryCache>(),
                    loggerFactory.CreateLogger<MemoryCacheService>());
            }
            catch (Exception ex)
            {
                var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
                loggerFactory.CreateLogger<MemoryCacheService>()
                    .LogError(ex, "Cache initialization failed. Using in-memory cache.");
                return new MemoryCacheService(
                    sp.GetRequiredService<Microsoft.Extensions.Caching.Memory.IMemoryCache>(),
                    loggerFactory.CreateLogger<MemoryCacheService>());
            }
        });
        services.AddScoped<IFileStorageService, FileStorageService>();
        services.AddScoped<INewsService, NewsService>();
        services.AddScoped<IGalleryService, GalleryService>();
        services.AddScoped<IVideoService, VideoService>();
        services.AddScoped<IPublicationService, PublicationService>();
        services.AddScoped<IOfficeBearerService, OfficeBearerService>();
        services.AddScoped<IAdvertisementService, AdvertisementService>();
        services.AddScoped<IBannerService, BannerService>();
        services.AddScoped<IMenuService, MenuService>();
        services.AddScoped<IPageService, PageService>();
        services.AddScoped<IContactService, ContactService>();
        services.AddScoped<IProjectInquiryService, ProjectInquiryService>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IServiceOfferingService, ServiceOfferingService>();
        services.AddScoped<ISectorService, SectorService>();
        services.AddScoped<IVisitorService, VisitorService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<ISiteSettingsService, SiteSettingsService>();

        return services;
    }
}
