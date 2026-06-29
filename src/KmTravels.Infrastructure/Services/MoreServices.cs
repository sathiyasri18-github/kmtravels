using Microsoft.EntityFrameworkCore;
using KmTravels.Core.DTOs;
using KmTravels.Core.Entities;
using KmTravels.Core.Enums;
using KmTravels.Core.Interfaces;
using KmTravels.Infrastructure.Data;

namespace KmTravels.Infrastructure.Services;

public class AdvertisementService : IAdvertisementService
{
    private readonly ApplicationDbContext _db;
    public AdvertisementService(ApplicationDbContext db) => _db = db;

    public async Task<IReadOnlyList<AdvertisementDto>> GetActiveAsync(AdvertisementPosition? position = null)
    {
        var now = DateTime.UtcNow;
        var query = _db.Advertisements.AsNoTracking()
            .Where(x => x.IsActive && x.StartDate <= now && x.EndDate >= now);
        if (position.HasValue) query = query.Where(x => x.Position == position);
        return await query.OrderBy(x => x.SortOrder)
            .Select(x => new AdvertisementDto(x.Id, x.Title, x.ImageUrl, x.LinkUrl, x.Position, x.StartDate, x.EndDate, x.SortOrder))
            .ToListAsync();
    }

    public async Task<IReadOnlyList<AdvertisementDto>> GetAllAsync() =>
        await _db.Advertisements.AsNoTracking().Where(x => x.IsActive)
            .Select(x => new AdvertisementDto(x.Id, x.Title, x.ImageUrl, x.LinkUrl, x.Position, x.StartDate, x.EndDate, x.SortOrder))
            .ToListAsync();

    public async Task<Advertisement> CreateAsync(AdvertisementDto dto)
    {
        var ad = new Advertisement { Title = dto.Title, ImageUrl = dto.ImageUrl, LinkUrl = dto.LinkUrl, Position = dto.Position, StartDate = dto.StartDate, EndDate = dto.EndDate, SortOrder = dto.SortOrder };
        _db.Advertisements.Add(ad);
        await _db.SaveChangesAsync();
        return ad;
    }

    public async Task<Advertisement?> UpdateAsync(int id, AdvertisementDto dto)
    {
        var ad = await _db.Advertisements.FindAsync(id);
        if (ad == null) return null;
        ad.Title = dto.Title; ad.ImageUrl = dto.ImageUrl; ad.LinkUrl = dto.LinkUrl;
        ad.Position = dto.Position; ad.StartDate = dto.StartDate; ad.EndDate = dto.EndDate; ad.SortOrder = dto.SortOrder;
        ad.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return ad;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var ad = await _db.Advertisements.FindAsync(id);
        if (ad == null) return false;
        ad.IsActive = false;
        await _db.SaveChangesAsync();
        return true;
    }
}

public class BannerService : IBannerService
{
    private readonly ApplicationDbContext _db;
    public BannerService(ApplicationDbContext db) => _db = db;

    public async Task<IReadOnlyList<BannerSlideDto>> GetActiveAsync() =>
        await _db.BannerSlides.AsNoTracking().Where(x => x.IsActive)
            .OrderBy(x => x.SortOrder)
            .Select(x => new BannerSlideDto(x.Id, x.Title, x.Subtitle, x.ImageUrl, x.LinkUrl, x.SortOrder))
            .ToListAsync();

    public async Task<IReadOnlyList<BannerSlideDto>> GetAllAsync() =>
        await _db.BannerSlides.AsNoTracking().Where(x => x.IsActive)
            .Select(x => new BannerSlideDto(x.Id, x.Title, x.Subtitle, x.ImageUrl, x.LinkUrl, x.SortOrder))
            .ToListAsync();

    public async Task<BannerSlide> CreateAsync(BannerSlideDto dto)
    {
        var slide = new BannerSlide { Title = dto.Title, Subtitle = dto.Subtitle, ImageUrl = dto.ImageUrl, LinkUrl = dto.LinkUrl, SortOrder = dto.SortOrder };
        _db.BannerSlides.Add(slide);
        await _db.SaveChangesAsync();
        return slide;
    }

    public async Task<BannerSlide?> UpdateAsync(int id, BannerSlideDto dto)
    {
        var slide = await _db.BannerSlides.FindAsync(id);
        if (slide == null) return null;
        slide.Title = dto.Title; slide.Subtitle = dto.Subtitle; slide.ImageUrl = dto.ImageUrl;
        slide.LinkUrl = dto.LinkUrl; slide.SortOrder = dto.SortOrder; slide.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return slide;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var slide = await _db.BannerSlides.FindAsync(id);
        if (slide == null) return false;
        slide.IsActive = false;
        await _db.SaveChangesAsync();
        return true;
    }
}

public class MenuService : IMenuService
{
    private const string MenuCacheKey = "menu:tree";
    private readonly ApplicationDbContext _db;
    private readonly ICacheService _cache;

    public MenuService(ApplicationDbContext db, ICacheService cache)
    {
        _db = db;
        _cache = cache;
    }

    public async Task<IReadOnlyList<MenuItemDto>> GetMenuTreeAsync()
    {
        var cached = await _cache.GetAsync<IReadOnlyList<MenuItemDto>>(MenuCacheKey);
        if (cached != null) return cached;

        var items = await _db.MenuItems.AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.SortOrder)
            .ToListAsync();

        var tree = BuildTree(items, null);
        await _cache.SetAsync(MenuCacheKey, tree);
        return tree;
    }

    private static List<MenuItemDto> BuildTree(List<MenuItem> items, int? parentId) =>
        items.Where(x => x.ParentId == parentId)
            .Select(x => new MenuItemDto(x.Id, x.Title, x.Url, x.ParentId, x.SortOrder, x.OpenInNewTab, BuildTree(items, x.Id)))
            .ToList();

    public async Task<MenuItem> CreateAsync(MenuItemDto dto)
    {
        var item = new MenuItem { Title = dto.Title, Url = dto.Url, ParentId = dto.ParentId, SortOrder = dto.SortOrder, OpenInNewTab = dto.OpenInNewTab };
        _db.MenuItems.Add(item);
        await _db.SaveChangesAsync();
        await _cache.RemoveAsync(MenuCacheKey);
        return item;
    }

    public async Task<MenuItem?> UpdateAsync(int id, MenuItemDto dto)
    {
        var item = await _db.MenuItems.FindAsync(id);
        if (item == null) return null;
        item.Title = dto.Title; item.Url = dto.Url; item.ParentId = dto.ParentId;
        item.SortOrder = dto.SortOrder; item.OpenInNewTab = dto.OpenInNewTab; item.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        await _cache.RemoveAsync(MenuCacheKey);
        return item;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var item = await _db.MenuItems.FindAsync(id);
        if (item == null) return false;
        item.IsActive = false;
        await _db.SaveChangesAsync();
        await _cache.RemoveAsync(MenuCacheKey);
        return true;
    }
}

public class PageService : IPageService
{
    private readonly ApplicationDbContext _db;
    public PageService(ApplicationDbContext db) => _db = db;

    public async Task<DynamicPageDto?> GetBySlugAsync(string slug) =>
        await _db.DynamicPages.AsNoTracking()
            .Where(x => x.Slug == slug && x.IsPublished && x.IsActive)
            .Select(x => new DynamicPageDto(x.Id, x.Title, x.Slug, x.Content, x.MetaDescription, x.IsPublished))
            .FirstOrDefaultAsync();

    public async Task<IReadOnlyList<DynamicPageDto>> GetAllAsync() =>
        await _db.DynamicPages.AsNoTracking().Where(x => x.IsActive)
            .Select(x => new DynamicPageDto(x.Id, x.Title, x.Slug, x.Content, x.MetaDescription, x.IsPublished))
            .ToListAsync();

    public async Task<DynamicPage> CreateAsync(DynamicPageDto dto)
    {
        var page = new DynamicPage { Title = dto.Title, Slug = dto.Slug, Content = dto.Content, MetaDescription = dto.MetaDescription, IsPublished = dto.IsPublished };
        _db.DynamicPages.Add(page);
        await _db.SaveChangesAsync();
        return page;
    }

    public async Task<DynamicPage?> UpdateAsync(int id, DynamicPageDto dto)
    {
        var page = await _db.DynamicPages.FindAsync(id);
        if (page == null) return null;
        page.Title = dto.Title; page.Slug = dto.Slug; page.Content = dto.Content;
        page.MetaDescription = dto.MetaDescription; page.IsPublished = dto.IsPublished; page.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return page;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var page = await _db.DynamicPages.FindAsync(id);
        if (page == null) return false;
        page.IsActive = false;
        await _db.SaveChangesAsync();
        return true;
    }
}

public class ContactService : IContactService
{
    private readonly ApplicationDbContext _db;
    public ContactService(ApplicationDbContext db) => _db = db;

    public async Task<ContactInquiry> SubmitAsync(SubmitContactRequest request)
    {
        var inquiry = new ContactInquiry { Name = request.Name, Email = request.Email, Phone = request.Phone, Subject = request.Subject, Message = request.Message };
        _db.ContactInquiries.Add(inquiry);
        await _db.SaveChangesAsync();
        return inquiry;
    }

    public async Task<PagedResult<ContactInquiryDto>> GetAllAsync(int page, int pageSize)
    {
        var total = await _db.ContactInquiries.CountAsync(x => x.IsActive);
        var items = await _db.ContactInquiries.AsNoTracking().Where(x => x.IsActive)
            .OrderByDescending(x => x.CreatedAt).Skip((page - 1) * pageSize).Take(pageSize)
            .Select(x => new ContactInquiryDto(x.Id, x.Name, x.Email, x.Phone, x.Subject, x.Message, x.IsRead, x.CreatedAt))
            .ToListAsync();
        return new PagedResult<ContactInquiryDto>(items, total, page, pageSize);
    }

    public async Task<bool> MarkAsReadAsync(int id)
    {
        var inquiry = await _db.ContactInquiries.FindAsync(id);
        if (inquiry == null) return false;
        inquiry.IsRead = true;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var inquiry = await _db.ContactInquiries.FindAsync(id);
        if (inquiry == null) return false;
        inquiry.IsActive = false;
        await _db.SaveChangesAsync();
        return true;
    }
}

public class ProjectInquiryService : IProjectInquiryService
{
    private readonly ApplicationDbContext _db;
    public ProjectInquiryService(ApplicationDbContext db) => _db = db;

    public async Task<MemberRegistration> SubmitAsync(SubmitMemberRequest request)
    {
        var inquiry = new MemberRegistration
        {
            MemberType = request.MemberType, FullName = request.FullName, Email = request.Email,
            Phone = request.Phone, Address = request.Address, District = request.District, State = request.State,
            CompanyName = request.CompanyName, LicenseNumber = request.LicenseNumber, DocumentUrl = request.DocumentUrl
        };
        _db.MemberRegistrations.Add(inquiry);
        await _db.SaveChangesAsync();
        return inquiry;
    }

    public async Task<PagedResult<ProjectInquiryDto>> GetAllAsync(int page, int pageSize, MemberStatus? status = null)
    {
        var query = _db.MemberRegistrations.AsNoTracking().Where(x => x.IsActive);
        if (status.HasValue) query = query.Where(x => x.Status == status);
        var total = await query.CountAsync();
        var items = await query.OrderByDescending(x => x.CreatedAt).Skip((page - 1) * pageSize).Take(pageSize)
            .Select(x => new ProjectInquiryDto(x.Id, x.FullName, x.Email, x.Phone, x.CompanyName, x.District, x.State, x.LicenseNumber, x.Address, x.Status, x.Notes, x.CreatedAt))
            .ToListAsync();
        return new PagedResult<ProjectInquiryDto>(items, total, page, pageSize);
    }

    public async Task<ProjectInquiryDto?> GetByIdAsync(int id)
    {
        var inquiry = await _db.MemberRegistrations.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
        return inquiry == null ? null : ToDto(inquiry);
    }

    public async Task<ProjectInquiryDto> CreateAsync(CreateProjectInquiryRequest request)
    {
        var inquiry = new MemberRegistration
        {
            MemberType = MemberType.Operator,
            Status = request.Status,
            FullName = request.FullName,
            Email = request.Email,
            Phone = request.Phone,
            CompanyName = request.CompanyName,
            District = request.City,
            State = request.State,
            LicenseNumber = request.ProjectType,
            Address = request.ProjectDetails,
            Notes = request.Notes
        };
        _db.MemberRegistrations.Add(inquiry);
        await _db.SaveChangesAsync();
        return ToDto(inquiry);
    }

    public async Task<ProjectInquiryDto?> UpdateAsync(int id, UpdateProjectInquiryRequest request)
    {
        var inquiry = await _db.MemberRegistrations.FindAsync(id);
        if (inquiry == null || !inquiry.IsActive) return null;

        inquiry.FullName = request.FullName;
        inquiry.Email = request.Email;
        inquiry.Phone = request.Phone;
        inquiry.CompanyName = request.CompanyName;
        inquiry.District = request.City;
        inquiry.State = request.State;
        inquiry.LicenseNumber = request.ProjectType;
        inquiry.Address = request.ProjectDetails;
        inquiry.Status = request.Status;
        inquiry.Notes = request.Notes;
        inquiry.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return ToDto(inquiry);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var inquiry = await _db.MemberRegistrations.FindAsync(id);
        if (inquiry == null) return false;
        inquiry.IsActive = false;
        await _db.SaveChangesAsync();
        return true;
    }

    private static ProjectInquiryDto ToDto(MemberRegistration x) =>
        new(x.Id, x.FullName, x.Email, x.Phone, x.CompanyName, x.District, x.State, x.LicenseNumber, x.Address, x.Status, x.Notes, x.CreatedAt);
}

public class EventService : IEventService
{
    private readonly ApplicationDbContext _db;
    public EventService(ApplicationDbContext db) => _db = db;

    public async Task<IReadOnlyList<EventDto>> GetPublishedAsync() =>
        await _db.Events.AsNoTracking().Where(x => x.IsPublished && x.IsActive)
            .OrderByDescending(x => x.StartDate)
            .Select(x => new EventDto(x.Id, x.Title, x.Description, x.Location, x.StartDate, x.EndDate, x.ImageUrl, x.IsPublished))
            .ToListAsync();

    public async Task<IReadOnlyList<EventDto>> GetAllAsync() =>
        await _db.Events.AsNoTracking().Where(x => x.IsActive)
            .Select(x => new EventDto(x.Id, x.Title, x.Description, x.Location, x.StartDate, x.EndDate, x.ImageUrl, x.IsPublished))
            .ToListAsync();

    public async Task<EventItem> CreateAsync(EventDto dto)
    {
        var evt = new EventItem { Title = dto.Title, Description = dto.Description, Location = dto.Location, StartDate = dto.StartDate, EndDate = dto.EndDate, ImageUrl = dto.ImageUrl, IsPublished = dto.IsPublished };
        _db.Events.Add(evt);
        await _db.SaveChangesAsync();
        return evt;
    }

    public async Task<EventItem?> UpdateAsync(int id, EventDto dto)
    {
        var evt = await _db.Events.FindAsync(id);
        if (evt == null) return null;
        evt.Title = dto.Title; evt.Description = dto.Description; evt.Location = dto.Location;
        evt.StartDate = dto.StartDate; evt.EndDate = dto.EndDate; evt.ImageUrl = dto.ImageUrl;
        evt.IsPublished = dto.IsPublished; evt.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return evt;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var evt = await _db.Events.FindAsync(id);
        if (evt == null) return false;
        evt.IsActive = false;
        await _db.SaveChangesAsync();
        return true;
    }
}

public class VisitorService : IVisitorService
{
    private readonly ApplicationDbContext _db;
    public VisitorService(ApplicationDbContext db) => _db = db;

    public async Task<VisitorStatsDto> GetStatsAsync()
    {
        var stats = await _db.VisitorStats.AsNoTracking().FirstOrDefaultAsync() ?? new VisitorStat();
        return new VisitorStatsDto(stats.TotalVisitors, stats.TotalPageViews);
    }

    public async Task RecordVisitAsync()
    {
        var stats = await _db.VisitorStats.FirstOrDefaultAsync();
        if (stats == null)
        {
            stats = new VisitorStat { TotalVisitors = 1, TotalPageViews = 0 };
            _db.VisitorStats.Add(stats);
        }
        else
        {
            stats.TotalVisitors++;
            stats.LastUpdated = DateTime.UtcNow;
        }
        await _db.SaveChangesAsync();
    }

    public async Task RecordPageViewAsync()
    {
        var stats = await _db.VisitorStats.FirstOrDefaultAsync();
        if (stats == null)
        {
            stats = new VisitorStat { TotalVisitors = 0, TotalPageViews = 1 };
            _db.VisitorStats.Add(stats);
        }
        else
        {
            stats.TotalPageViews++;
            stats.LastUpdated = DateTime.UtcNow;
        }
        await _db.SaveChangesAsync();
    }
}

public class DashboardService : IDashboardService
{
    private readonly ApplicationDbContext _db;
    public DashboardService(ApplicationDbContext db) => _db = db;

    public async Task<DashboardStatsDto> GetStatsAsync()
    {
        var visitor = await _db.VisitorStats.AsNoTracking().FirstOrDefaultAsync();
        return new DashboardStatsDto(
            visitor?.TotalVisitors ?? 0,
            visitor?.TotalPageViews ?? 0,
            await _db.NewsArticles.CountAsync(x => x.IsActive),
            await _db.MemberRegistrations.CountAsync(x => x.IsActive),
            await _db.MemberRegistrations.CountAsync(x => x.IsActive && x.Status == MemberStatus.Pending),
            await _db.ContactInquiries.CountAsync(x => x.IsActive),
            await _db.ContactInquiries.CountAsync(x => x.IsActive && !x.IsRead)
        );
    }
}

public class SiteSettingsService : ISiteSettingsService
{
    private readonly ApplicationDbContext _db;
    private readonly ICacheService _cache;

    public SiteSettingsService(ApplicationDbContext db, ICacheService cache)
    {
        _db = db;
        _cache = cache;
    }

    public async Task<SiteSettingsDto> GetAsync()
    {
        var cached = await _cache.GetAsync<SiteSettingsDto>("site:settings");
        if (cached != null) return cached;

        var settings = await _db.SiteSettings.AsNoTracking().ToDictionaryAsync(x => x.Key, x => x.Value);
        var dto = new SiteSettingsDto(
            settings.GetValueOrDefault("AssociationName", "SNL Engineering"),
            settings.GetValueOrDefault("Tagline", "Uniting Cable TV Operators Across Tamil Nadu"),
            settings.GetValueOrDefault("Address", "Chennai, Tamil Nadu, India"),
            settings.GetValueOrDefault("Phone", "+91-44-00000000"),
            settings.GetValueOrDefault("Email", "info@KmTravels.org"),
            settings.GetValueOrDefault("MapEmbedUrl", null),
            settings.GetValueOrDefault("AboutContent", null),
            settings.GetValueOrDefault("Vision", null),
            settings.GetValueOrDefault("Mission", null),
            settings.GetValueOrDefault("Objectives", null)
        );
        await _cache.SetAsync("site:settings", dto, TimeSpan.FromHours(1));
        return dto;
    }

    public async Task UpdateAsync(SiteSettingsDto settings)
    {
        var dict = new Dictionary<string, string?>
        {
            ["AssociationName"] = settings.AssociationName, ["Tagline"] = settings.Tagline,
            ["Address"] = settings.Address, ["Phone"] = settings.Phone, ["Email"] = settings.Email,
            ["MapEmbedUrl"] = settings.MapEmbedUrl, ["AboutContent"] = settings.AboutContent,
            ["Vision"] = settings.Vision, ["Mission"] = settings.Mission, ["Objectives"] = settings.Objectives
        };

        foreach (var (key, value) in dict)
        {
            if (value == null) continue;
            var setting = await _db.SiteSettings.FirstOrDefaultAsync(x => x.Key == key);
            if (setting == null)
            {
                _db.SiteSettings.Add(new SiteSetting { Key = key, Value = value });
            }
            else
            {
                setting.Value = value;
                setting.UpdatedAt = DateTime.UtcNow;
            }
        }
        await _db.SaveChangesAsync();
        await _cache.RemoveAsync("site:settings");
    }
}
