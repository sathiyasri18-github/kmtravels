using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnlEngineering.Core.DTOs;
using SnlEngineering.Core.Enums;
using SnlEngineering.Core.Interfaces;

namespace SnlEngineering.Api.Controllers;

[ApiController]
[Route("api/public")]
[AllowAnonymous]
public class HomeController : ControllerBase
{
    private readonly IBannerService _banners;
    private readonly INewsService _news;
    private readonly IAdvertisementService _ads;
    private readonly IEventService _events;
    private readonly ISiteSettingsService _settings;
    private readonly IVisitorService _visitors;
    private readonly IServiceOfferingService _services;

    public HomeController(IBannerService banners, INewsService news, IAdvertisementService ads, IEventService events, ISiteSettingsService settings, IVisitorService visitors, IServiceOfferingService services)
    {
        _banners = banners; _news = news; _ads = ads; _events = events; _settings = settings; _visitors = visitors; _services = services;
    }

    [HttpGet]
    public async Task<IActionResult> GetHomeData()
    {
        await _visitors.RecordVisitAsync();
        return Ok(new ApiResponse<object>(true, new
        {
            Banners = await _banners.GetActiveAsync(),
            FeaturedNews = (await _news.GetPublishedAsync(1, 6)).Items,
            Advertisements = await _ads.GetActiveAsync(),
            UpcomingEvents = (await _events.GetPublishedAsync()).Take(3),
            Settings = await _settings.GetAsync(),
            VisitorStats = await _visitors.GetStatsAsync(),
            Services = await _services.GetPublishedAsync(10)
        }));
    }

    [HttpPost("pageview")]
    public async Task<IActionResult> RecordPageView()
    {
        await _visitors.RecordPageViewAsync();
        return Ok();
    }
}

[ApiController]
[Route("api/public/news")]
[AllowAnonymous]
public class PublicNewsController : ControllerBase
{
    private readonly INewsService _news;
    public PublicNewsController(INewsService news) => _news = news;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] NewsCategory? category = null)
        => Ok(new ApiResponse<PagedResult<NewsDto>>(true, await _news.GetPublishedAsync(page, pageSize, category)));

    [HttpGet("{slug}")]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        var article = await _news.GetBySlugAsync(slug);
        return article == null ? NotFound() : Ok(new ApiResponse<NewsDetailDto>(true, article));
    }

    [HttpGet("slugs")]
    public async Task<IActionResult> GetSlugs()
    {
        var all = await _news.GetPublishedAsync(1, 1000);
        var slugs = all.Items.Select(x => new { x.Slug }).ToList();
        return Ok(new ApiResponse<object>(true, slugs));
    }
}

[ApiController]
[Route("api/public/gallery")]
[AllowAnonymous]
public class PublicGalleryController : ControllerBase
{
    private readonly IGalleryService _gallery;
    public PublicGalleryController(IGalleryService gallery) => _gallery = gallery;

    [HttpGet]
    public async Task<IActionResult> GetAlbums() => Ok(new ApiResponse<IReadOnlyList<GalleryAlbumDto>>(true, await _gallery.GetAlbumsAsync()));

    [HttpGet("slideshow")]
    public async Task<IActionResult> GetSlideshow()
        => Ok(new ApiResponse<IReadOnlyList<GalleryImageDto>>(true, await _gallery.GetSlideshowImagesAsync()));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAlbum(int id)
    {
        var album = await _gallery.GetAlbumAsync(id);
        if (album == null) return NotFound();
        var images = await _gallery.GetImagesAsync(id);
        return Ok(new ApiResponse<object>(true, new { Album = album, Images = images }));
    }
}

[ApiController]
[Route("api/public/services")]
[AllowAnonymous]
public class PublicServicesController : ControllerBase
{
    private readonly IServiceOfferingService _services;
    public PublicServicesController(IServiceOfferingService services) => _services = services;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? limit = null)
        => Ok(new ApiResponse<IReadOnlyList<ServiceDto>>(true, await _services.GetPublishedAsync(limit)));

    [HttpGet("{slug}")]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        var service = await _services.GetBySlugAsync(slug);
        return service == null ? NotFound() : Ok(new ApiResponse<ServiceDetailDto>(true, service));
    }

    [HttpGet("slugs")]
    public async Task<IActionResult> GetSlugs()
    {
        var all = await _services.GetPublishedAsync(null);
        var slugs = all.Select(x => new { x.Slug }).ToList();
        return Ok(new ApiResponse<object>(true, slugs));
    }
}

[ApiController]
[Route("api/public/videos")]
[AllowAnonymous]
public class PublicVideosController : ControllerBase
{
    private readonly IVideoService _videos;
    public PublicVideosController(IVideoService videos) => _videos = videos;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(new ApiResponse<IReadOnlyList<VideoDto>>(true, await _videos.GetPublishedAsync()));
}

[ApiController]
[Route("api/public/sectors")]
[AllowAnonymous]
public class PublicSectorsController : ControllerBase
{
    private readonly ISectorService _sectors;
    public PublicSectorsController(ISectorService sectors) => _sectors = sectors;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? limit = null)
        => Ok(new ApiResponse<IReadOnlyList<SectorDto>>(true, await _sectors.GetPublishedAsync(limit)));

    [HttpGet("{slug}")]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        var sector = await _sectors.GetBySlugAsync(slug);
        return sector == null ? NotFound() : Ok(new ApiResponse<SectorDetailDto>(true, sector));
    }

    [HttpGet("slugs")]
    public async Task<IActionResult> GetSlugs()
    {
        var all = await _sectors.GetPublishedAsync(null);
        var slugs = all.Select(x => new { x.Slug }).ToList();
        return Ok(new ApiResponse<object>(true, slugs));
    }
}

[ApiController]
[Route("api/public/office-bearers")]
[AllowAnonymous]
public class PublicOfficeBearersController : ControllerBase
{
    private readonly IOfficeBearerService _bearers;
    public PublicOfficeBearersController(IOfficeBearerService bearers) => _bearers = bearers;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(new ApiResponse<IReadOnlyList<OfficeBearerDto>>(true, await _bearers.GetAllAsync()));
}

[ApiController]
[Route("api/public/about")]
[AllowAnonymous]
public class PublicAboutController : ControllerBase
{
    private readonly ISiteSettingsService _settings;
    public PublicAboutController(ISiteSettingsService settings) => _settings = settings;

    [HttpGet]
    public async Task<IActionResult> Get() => Ok(new ApiResponse<SiteSettingsDto>(true, await _settings.GetAsync()));
}

[ApiController]
[Route("api/public/contact")]
[AllowAnonymous]
public class PublicContactController : ControllerBase
{
    private readonly IContactService _contact;
    private readonly ISiteSettingsService _settings;
    public PublicContactController(IContactService contact, ISiteSettingsService settings) { _contact = contact; _settings = settings; }

    [HttpGet]
    public async Task<IActionResult> GetInfo() => Ok(new ApiResponse<SiteSettingsDto>(true, await _settings.GetAsync()));

    [HttpPost]
    public async Task<IActionResult> Submit([FromBody] SubmitContactRequest request)
    {
        var inquiry = await _contact.SubmitAsync(request);
        return Ok(new ApiResponse<object>(true, new { inquiry.Id }, "Thank you for your inquiry. We will respond shortly."));
    }
}

[ApiController]
[Route("api/public/membership")]
[Route("api/public/project-inquiries")]
[AllowAnonymous]
public class PublicProjectInquiryController : ControllerBase
{
    private readonly IProjectInquiryService _inquiries;
    public PublicProjectInquiryController(IProjectInquiryService inquiries) => _inquiries = inquiries;

    [HttpPost("register")]
    [HttpPost]
    public async Task<IActionResult> Submit([FromBody] SubmitMemberRequest request)
    {
        var inquiry = await _inquiries.SubmitAsync(request);
        return Ok(new ApiResponse<object>(true, new { inquiry.Id }, "Thank you for your enquiry. We will respond shortly."));
    }
}

[ApiController]
[Route("api/public/menu")]
[AllowAnonymous]
public class PublicMenuController : ControllerBase
{
    private readonly IMenuService _menu;
    public PublicMenuController(IMenuService menu) => _menu = menu;

    [HttpGet]
    public async Task<IActionResult> Get() => Ok(new ApiResponse<IReadOnlyList<MenuItemDto>>(true, await _menu.GetMenuTreeAsync()));
}

[ApiController]
[Route("api/public/events")]
[AllowAnonymous]
public class PublicEventsController : ControllerBase
{
    private readonly IEventService _events;
    public PublicEventsController(IEventService events) => _events = events;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(new ApiResponse<IReadOnlyList<EventDto>>(true, await _events.GetPublishedAsync()));
}

[ApiController]
[Route("api/public/pages/{slug}")]
[AllowAnonymous]
public class PublicPagesController : ControllerBase
{
    private readonly IPageService _pages;
    public PublicPagesController(IPageService pages) => _pages = pages;

    [HttpGet]
    public async Task<IActionResult> Get(string slug)
    {
        var page = await _pages.GetBySlugAsync(slug);
        return page == null ? NotFound() : Ok(new ApiResponse<DynamicPageDto>(true, page));
    }
}
