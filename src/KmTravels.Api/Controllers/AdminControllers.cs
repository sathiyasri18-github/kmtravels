using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KmTravels.Core.DTOs;
using KmTravels.Core.Enums;
using KmTravels.Core.Interfaces;

namespace KmTravels.Api.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Policy = "AdminOnly")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboard;
    public DashboardController(IDashboardService dashboard) => _dashboard = dashboard;

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetStats() => Ok(new ApiResponse<DashboardStatsDto>(true, await _dashboard.GetStatsAsync()));
}

[ApiController]
[Route("api/admin/news")]
[Authorize(Policy = "AdminOnly")]
public class AdminNewsController : ControllerBase
{
    private readonly INewsService _news;
    public AdminNewsController(INewsService news) => _news = news;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(new ApiResponse<PagedResult<NewsDto>>(true, await _news.GetAllAsync(page, pageSize)));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var article = await _news.GetByIdAsync(id);
        return article == null ? NotFound() : Ok(new ApiResponse<NewsDetailDto>(true, article));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateNewsRequest request)
        => Ok(new ApiResponse<object>(true, await _news.CreateAsync(request)));

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateNewsRequest request)
    {
        var article = await _news.UpdateAsync(id, request);
        return article == null ? NotFound() : Ok(new ApiResponse<object>(true, article));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) => Ok(new ApiResponse<bool>(true, await _news.DeleteAsync(id)));

    [HttpPatch("{id:int}/publish")]
    public async Task<IActionResult> TogglePublish(int id) => Ok(new ApiResponse<bool>(true, await _news.TogglePublishAsync(id)));
}

[ApiController]
[Route("api/admin/services")]
[Authorize(Policy = "AdminOnly")]
public class AdminServicesController : ControllerBase
{
    private readonly IServiceOfferingService _services;
    public AdminServicesController(IServiceOfferingService services) => _services = services;

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(new ApiResponse<IReadOnlyList<ServiceDto>>(true, await _services.GetAllAsync()));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var service = await _services.GetByIdAsync(id);
        return service == null ? NotFound() : Ok(new ApiResponse<ServiceDetailDto>(true, service));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateServiceRequest request)
        => Ok(new ApiResponse<ServiceDetailDto>(true, await _services.CreateAsync(request)));

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateServiceRequest request)
    {
        var service = await _services.UpdateAsync(id, request);
        return service == null ? NotFound() : Ok(new ApiResponse<ServiceDetailDto>(true, service));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) => Ok(new ApiResponse<bool>(true, await _services.DeleteAsync(id)));

    [HttpPatch("{id:int}/publish")]
    public async Task<IActionResult> TogglePublish(int id) => Ok(new ApiResponse<bool>(true, await _services.TogglePublishAsync(id)));
}

[ApiController]
[Route("api/admin/gallery")]
[Authorize(Policy = "AdminOnly")]
public class AdminGalleryController : ControllerBase
{
    private readonly IGalleryService _gallery;
    public AdminGalleryController(IGalleryService gallery) => _gallery = gallery;

    [HttpGet]
    public async Task<IActionResult> GetAlbums() => Ok(new ApiResponse<IReadOnlyList<GalleryAlbumDto>>(true, await _gallery.GetAlbumsAsync()));

    [HttpPost]
    public async Task<IActionResult> CreateAlbum([FromBody] CreateAlbumRequest request)
        => Ok(new ApiResponse<object>(true, await _gallery.CreateAlbumAsync(request)));

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAlbum(int id, [FromBody] CreateAlbumRequest request)
    {
        var album = await _gallery.UpdateAlbumAsync(id, request);
        return album == null ? NotFound() : Ok(new ApiResponse<object>(true, album));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAlbum(int id) => Ok(new ApiResponse<bool>(true, await _gallery.DeleteAlbumAsync(id)));

    [HttpPost("{albumId:int}/images")]
    public async Task<IActionResult> AddImage(int albumId, [FromBody] GalleryImageDto dto)
        => Ok(new ApiResponse<object>(true, await _gallery.AddImageAsync(albumId, dto.Title, dto.ImageUrl, dto.Caption, dto.SortOrder)));

    [HttpDelete("images/{imageId:int}")]
    public async Task<IActionResult> DeleteImage(int imageId) => Ok(new ApiResponse<bool>(true, await _gallery.DeleteImageAsync(imageId)));
}

[ApiController]
[Route("api/admin/videos")]
[Authorize(Policy = "AdminOnly")]
public class AdminVideosController : ControllerBase
{
    private readonly IVideoService _videos;
    public AdminVideosController(IVideoService videos) => _videos = videos;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(new ApiResponse<IReadOnlyList<VideoDto>>(true, await _videos.GetAllAsync()));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateVideoRequest request)
        => Ok(new ApiResponse<object>(true, await _videos.CreateAsync(request)));

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateVideoRequest request)
    {
        var video = await _videos.UpdateAsync(id, request);
        return video == null ? NotFound() : Ok(new ApiResponse<object>(true, video));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) => Ok(new ApiResponse<bool>(true, await _videos.DeleteAsync(id)));
}

[ApiController]
[Route("api/admin/sectors")]
[Authorize(Policy = "AdminOnly")]
public class AdminSectorsController : ControllerBase
{
    private readonly ISectorService _sectors;
    public AdminSectorsController(ISectorService sectors) => _sectors = sectors;

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(new ApiResponse<IReadOnlyList<SectorDto>>(true, await _sectors.GetAllAsync()));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var sector = await _sectors.GetByIdAsync(id);
        return sector == null ? NotFound() : Ok(new ApiResponse<SectorDetailDto>(true, sector));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSectorRequest request)
        => Ok(new ApiResponse<SectorDetailDto>(true, await _sectors.CreateAsync(request)));

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateSectorRequest request)
    {
        var sector = await _sectors.UpdateAsync(id, request);
        return sector == null ? NotFound() : Ok(new ApiResponse<SectorDetailDto>(true, sector));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) => Ok(new ApiResponse<bool>(true, await _sectors.DeleteAsync(id)));

    [HttpPatch("{id:int}/publish")]
    public async Task<IActionResult> TogglePublish(int id) => Ok(new ApiResponse<bool>(true, await _sectors.TogglePublishAsync(id)));
}

[ApiController]
[Route("api/admin/office-bearers")]
[Authorize(Policy = "AdminOnly")]
public class AdminOfficeBearersController : ControllerBase
{
    private readonly IOfficeBearerService _bearers;
    public AdminOfficeBearersController(IOfficeBearerService bearers) => _bearers = bearers;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(new ApiResponse<IReadOnlyList<OfficeBearerDto>>(true, await _bearers.GetAllAsync()));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOfficeBearerRequest request)
        => Ok(new ApiResponse<object>(true, await _bearers.CreateAsync(request)));

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateOfficeBearerRequest request)
    {
        var bearer = await _bearers.UpdateAsync(id, request);
        return bearer == null ? NotFound() : Ok(new ApiResponse<object>(true, bearer));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) => Ok(new ApiResponse<bool>(true, await _bearers.DeleteAsync(id)));
}

[ApiController]
[Route("api/admin/advertisements")]
[Authorize(Policy = "AdminOnly")]
public class AdminAdvertisementsController : ControllerBase
{
    private readonly IAdvertisementService _ads;
    public AdminAdvertisementsController(IAdvertisementService ads) => _ads = ads;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(new ApiResponse<IReadOnlyList<AdvertisementDto>>(true, await _ads.GetAllAsync()));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AdvertisementDto dto)
        => Ok(new ApiResponse<object>(true, await _ads.CreateAsync(dto)));

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] AdvertisementDto dto)
    {
        var ad = await _ads.UpdateAsync(id, dto);
        return ad == null ? NotFound() : Ok(new ApiResponse<object>(true, ad));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) => Ok(new ApiResponse<bool>(true, await _ads.DeleteAsync(id)));
}

[ApiController]
[Route("api/admin/banners")]
[Authorize(Policy = "AdminOnly")]
public class AdminBannersController : ControllerBase
{
    private readonly IBannerService _banners;
    public AdminBannersController(IBannerService banners) => _banners = banners;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(new ApiResponse<IReadOnlyList<BannerSlideDto>>(true, await _banners.GetAllAsync()));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BannerSlideDto dto)
        => Ok(new ApiResponse<object>(true, await _banners.CreateAsync(dto)));

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] BannerSlideDto dto)
    {
        var slide = await _banners.UpdateAsync(id, dto);
        return slide == null ? NotFound() : Ok(new ApiResponse<object>(true, slide));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) => Ok(new ApiResponse<bool>(true, await _banners.DeleteAsync(id)));
}

[ApiController]
[Route("api/admin/project-inquiries")]
[Authorize(Policy = "AdminOnly")]
public class AdminProjectInquiriesController : ControllerBase
{
    private readonly IProjectInquiryService _inquiries;
    public AdminProjectInquiriesController(IProjectInquiryService inquiries) => _inquiries = inquiries;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] MemberStatus? status = null)
        => Ok(new ApiResponse<PagedResult<ProjectInquiryDto>>(true, await _inquiries.GetAllAsync(page, pageSize, status)));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var inquiry = await _inquiries.GetByIdAsync(id);
        return inquiry == null ? NotFound() : Ok(new ApiResponse<ProjectInquiryDto>(true, inquiry));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProjectInquiryRequest request)
        => Ok(new ApiResponse<ProjectInquiryDto>(true, await _inquiries.CreateAsync(request)));

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProjectInquiryRequest request)
    {
        var inquiry = await _inquiries.UpdateAsync(id, request);
        return inquiry == null ? NotFound() : Ok(new ApiResponse<ProjectInquiryDto>(true, inquiry));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) => Ok(new ApiResponse<bool>(true, await _inquiries.DeleteAsync(id)));
}

[ApiController]
[Route("api/admin/contact")]
[Authorize(Policy = "AdminOnly")]
public class AdminContactController : ControllerBase
{
    private readonly IContactService _contact;
    public AdminContactController(IContactService contact) => _contact = contact;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(new ApiResponse<PagedResult<ContactInquiryDto>>(true, await _contact.GetAllAsync(page, pageSize)));

    [HttpPatch("{id:int}/read")]
    public async Task<IActionResult> MarkRead(int id) => Ok(new ApiResponse<bool>(true, await _contact.MarkAsReadAsync(id)));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) => Ok(new ApiResponse<bool>(true, await _contact.DeleteAsync(id)));
}

[ApiController]
[Route("api/admin/menu")]
[Authorize(Policy = "AdminOnly")]
public class AdminMenuController : ControllerBase
{
    private readonly IMenuService _menu;
    public AdminMenuController(IMenuService menu) => _menu = menu;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(new ApiResponse<IReadOnlyList<MenuItemDto>>(true, await _menu.GetMenuTreeAsync()));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MenuItemDto dto)
        => Ok(new ApiResponse<object>(true, await _menu.CreateAsync(dto)));

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] MenuItemDto dto)
    {
        var item = await _menu.UpdateAsync(id, dto);
        return item == null ? NotFound() : Ok(new ApiResponse<object>(true, item));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) => Ok(new ApiResponse<bool>(true, await _menu.DeleteAsync(id)));
}

[ApiController]
[Route("api/admin/pages")]
[Authorize(Policy = "AdminOnly")]
public class AdminPagesController : ControllerBase
{
    private readonly IPageService _pages;
    public AdminPagesController(IPageService pages) => _pages = pages;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(new ApiResponse<IReadOnlyList<DynamicPageDto>>(true, await _pages.GetAllAsync()));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DynamicPageDto dto)
        => Ok(new ApiResponse<object>(true, await _pages.CreateAsync(dto)));

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] DynamicPageDto dto)
    {
        var page = await _pages.UpdateAsync(id, dto);
        return page == null ? NotFound() : Ok(new ApiResponse<object>(true, page));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) => Ok(new ApiResponse<bool>(true, await _pages.DeleteAsync(id)));
}

[ApiController]
[Route("api/admin/events")]
[Authorize(Policy = "AdminOnly")]
public class AdminEventsController : ControllerBase
{
    private readonly IEventService _events;
    public AdminEventsController(IEventService events) => _events = events;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(new ApiResponse<IReadOnlyList<EventDto>>(true, await _events.GetAllAsync()));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EventDto dto)
        => Ok(new ApiResponse<object>(true, await _events.CreateAsync(dto)));

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] EventDto dto)
    {
        var evt = await _events.UpdateAsync(id, dto);
        return evt == null ? NotFound() : Ok(new ApiResponse<object>(true, evt));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) => Ok(new ApiResponse<bool>(true, await _events.DeleteAsync(id)));
}

[ApiController]
[Route("api/admin/settings")]
[Authorize(Policy = "SuperAdminOnly")]
public class AdminSettingsController : ControllerBase
{
    private readonly ISiteSettingsService _settings;
    public AdminSettingsController(ISiteSettingsService settings) => _settings = settings;

    [HttpGet]
    public async Task<IActionResult> Get() => Ok(new ApiResponse<SiteSettingsDto>(true, await _settings.GetAsync()));

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] SiteSettingsDto dto)
    {
        await _settings.UpdateAsync(dto);
        return Ok(new ApiResponse<bool>(true, true));
    }
}
