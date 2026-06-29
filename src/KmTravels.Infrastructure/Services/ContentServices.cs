using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using SnlEngineering.Core.DTOs;
using SnlEngineering.Core.Entities;
using SnlEngineering.Core.Enums;
using SnlEngineering.Core.Interfaces;
using SnlEngineering.Infrastructure.Data;

namespace SnlEngineering.Infrastructure.Services;

public partial class ContentServices
{
    protected readonly ApplicationDbContext _db;
    protected readonly ICacheService _cache;

    public ContentServices(ApplicationDbContext db, ICacheService cache)
    {
        _db = db;
        _cache = cache;
    }

    protected static string GenerateSlug(string title)
    {
        var slug = title.ToLowerInvariant().Trim();
        slug = NonAlphaRegex().Replace(slug, "");
        slug = SpaceRegex().Replace(slug, "-");
        return slug.Trim('-');
    }

    [GeneratedRegex(@"[^a-z0-9\s-]")]
    private static partial Regex NonAlphaRegex();

    [GeneratedRegex(@"\s+")]
    private static partial Regex SpaceRegex();
}

public class NewsService : ContentServices, INewsService
{
    public NewsService(ApplicationDbContext db, ICacheService cache) : base(db, cache) { }

    public async Task<PagedResult<NewsDto>> GetPublishedAsync(int page, int pageSize, NewsCategory? category = null)
    {
        var cacheKey = $"news:pub:{page}:{pageSize}:{category}";
        var cached = await _cache.GetAsync<PagedResult<NewsDto>>(cacheKey);
        if (cached != null) return cached;

        var query = _db.NewsArticles.AsNoTracking().Where(x => x.IsPublished && x.IsActive);
        if (category.HasValue) query = query.Where(x => x.Category == category);

        var total = await query.CountAsync();
        var items = await query.OrderByDescending(x => x.PublishedAt)
            .Skip((page - 1) * pageSize).Take(pageSize)
            .Select(x => new NewsDto(x.Id, x.Title, x.Slug, x.Summary, x.ImageUrl, x.Category, x.IsPublished, x.PublishedAt, x.Author, x.ViewCount))
            .ToListAsync();

        var result = new PagedResult<NewsDto>(items, total, page, pageSize);
        await _cache.SetAsync(cacheKey, result);
        return result;
    }

    public async Task<NewsDetailDto?> GetBySlugAsync(string slug)
    {
        var article = await _db.NewsArticles.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Slug == slug && x.IsPublished && x.IsActive);
        if (article == null) return null;

        await _db.NewsArticles.Where(x => x.Id == article.Id).ExecuteUpdateAsync(s => s.SetProperty(x => x.ViewCount, x => x.ViewCount + 1));

        return new NewsDetailDto(article.Id, article.Title, article.Slug, article.Summary, article.Content,
            article.ImageUrl, article.Category, article.PublishedAt, article.Author, article.ViewCount + 1);
    }

    public async Task<PagedResult<NewsDto>> GetAllAsync(int page, int pageSize)
    {
        var total = await _db.NewsArticles.CountAsync(x => x.IsActive);
        var items = await _db.NewsArticles.AsNoTracking().Where(x => x.IsActive)
            .OrderByDescending(x => x.CreatedAt).Skip((page - 1) * pageSize).Take(pageSize)
            .Select(x => new NewsDto(x.Id, x.Title, x.Slug, x.Summary, x.ImageUrl, x.Category, x.IsPublished, x.PublishedAt, x.Author, x.ViewCount))
            .ToListAsync();
        return new PagedResult<NewsDto>(items, total, page, pageSize);
    }

    public async Task<NewsDetailDto?> GetByIdAsync(int id)
    {
        var x = await _db.NewsArticles.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id && a.IsActive);
        return x == null ? null : new NewsDetailDto(x.Id, x.Title, x.Slug, x.Summary, x.Content, x.ImageUrl, x.Category, x.PublishedAt, x.Author, x.ViewCount);
    }

    public async Task<NewsArticle> CreateAsync(CreateNewsRequest request)
    {
        var slug = GenerateSlug(request.Title);
        var article = new NewsArticle
        {
            Title = request.Title, Slug = slug, Summary = request.Summary, Content = request.Content,
            ImageUrl = request.ImageUrl, Category = request.Category, IsPublished = request.IsPublished,
            PublishedAt = request.IsPublished ? DateTime.UtcNow : null, Author = request.Author
        };
        _db.NewsArticles.Add(article);
        await _db.SaveChangesAsync();
        await _cache.RemoveByPrefixAsync("news:");
        return article;
    }

    public async Task<NewsArticle?> UpdateAsync(int id, CreateNewsRequest request)
    {
        var article = await _db.NewsArticles.FindAsync(id);
        if (article == null) return null;
        article.Title = request.Title;
        article.Slug = GenerateSlug(request.Title);
        article.Summary = request.Summary;
        article.Content = request.Content;
        article.ImageUrl = request.ImageUrl;
        article.Category = request.Category;
        article.Author = request.Author;
        article.UpdatedAt = DateTime.UtcNow;
        if (request.IsPublished && !article.IsPublished) article.PublishedAt = DateTime.UtcNow;
        article.IsPublished = request.IsPublished;
        await _db.SaveChangesAsync();
        await _cache.RemoveByPrefixAsync("news:");
        return article;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var article = await _db.NewsArticles.FindAsync(id);
        if (article == null) return false;
        article.IsActive = false;
        await _db.SaveChangesAsync();
        await _cache.RemoveByPrefixAsync("news:");
        return true;
    }

    public async Task<bool> TogglePublishAsync(int id)
    {
        var article = await _db.NewsArticles.FindAsync(id);
        if (article == null) return false;
        article.IsPublished = !article.IsPublished;
        article.PublishedAt = article.IsPublished ? DateTime.UtcNow : article.PublishedAt;
        await _db.SaveChangesAsync();
        await _cache.RemoveByPrefixAsync("news:");
        return true;
    }
}

public class GalleryService : ContentServices, IGalleryService
{
    public GalleryService(ApplicationDbContext db, ICacheService cache) : base(db, cache) { }

    public async Task<IReadOnlyList<GalleryAlbumDto>> GetAlbumsAsync()
    {
        return await _db.GalleryAlbums.AsNoTracking().Where(x => x.IsActive)
            .OrderByDescending(x => x.EventDate ?? x.CreatedAt)
            .Select(x => new GalleryAlbumDto(x.Id, x.Title, x.Description, x.CoverImageUrl, x.Category, x.EventDate, x.Images.Count))
            .ToListAsync();
    }

    public async Task<GalleryAlbumDto?> GetAlbumAsync(int id)
    {
        return await _db.GalleryAlbums.AsNoTracking().Where(x => x.Id == id && x.IsActive)
            .Select(x => new GalleryAlbumDto(x.Id, x.Title, x.Description, x.CoverImageUrl, x.Category, x.EventDate, x.Images.Count))
            .FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<GalleryImageDto>> GetImagesAsync(int albumId) =>
        await _db.GalleryImages.AsNoTracking().Where(x => x.AlbumId == albumId && x.IsActive)
            .OrderBy(x => x.SortOrder).Select(x => new GalleryImageDto(x.Id, x.AlbumId, x.Title, x.ImageUrl, x.Caption, x.SortOrder)).ToListAsync();

    public async Task<IReadOnlyList<GalleryImageDto>> GetSlideshowImagesAsync(int limit = 10)
    {
        var slideshowAlbumIds = await _db.GalleryAlbums.AsNoTracking()
            .Where(x => x.IsActive && x.Category == "Slideshow")
            .Select(x => x.Id)
            .ToListAsync();

        var query = _db.GalleryImages.AsNoTracking()
            .Where(x => x.IsActive && x.Album.IsActive);

        if (slideshowAlbumIds.Count > 0)
            query = query.Where(x => slideshowAlbumIds.Contains(x.AlbumId));

        return await query
            .OrderBy(x => x.AlbumId)
            .ThenBy(x => x.SortOrder)
            .Take(limit)
            .Select(x => new GalleryImageDto(x.Id, x.AlbumId, x.Title, x.ImageUrl, x.Caption, x.SortOrder))
            .ToListAsync();
    }

    public async Task<GalleryAlbum> CreateAlbumAsync(CreateAlbumRequest request)
    {
        var album = new GalleryAlbum { Title = request.Title, Description = request.Description, Category = request.Category, EventDate = request.EventDate };
        _db.GalleryAlbums.Add(album);
        await _db.SaveChangesAsync();
        return album;
    }

    public async Task<GalleryAlbum?> UpdateAlbumAsync(int id, CreateAlbumRequest request)
    {
        var album = await _db.GalleryAlbums.FindAsync(id);
        if (album == null) return null;
        album.Title = request.Title; album.Description = request.Description;
        album.Category = request.Category; album.EventDate = request.EventDate; album.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return album;
    }

    public async Task<bool> DeleteAlbumAsync(int id)
    {
        var album = await _db.GalleryAlbums.FindAsync(id);
        if (album == null) return false;
        album.IsActive = false;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<GalleryImage> AddImageAsync(int albumId, string title, string imageUrl, string? caption, int sortOrder)
    {
        var image = new GalleryImage { AlbumId = albumId, Title = title, ImageUrl = imageUrl, Caption = caption, SortOrder = sortOrder };
        _db.GalleryImages.Add(image);
        var album = await _db.GalleryAlbums.FindAsync(albumId);
        if (album != null && album.CoverImageUrl == null) album.CoverImageUrl = imageUrl;
        await _db.SaveChangesAsync();
        return image;
    }

    public async Task<bool> DeleteImageAsync(int imageId)
    {
        var image = await _db.GalleryImages.FindAsync(imageId);
        if (image == null) return false;
        image.IsActive = false;
        await _db.SaveChangesAsync();
        return true;
    }
}

public class VideoService : ContentServices, IVideoService
{
    public VideoService(ApplicationDbContext db, ICacheService cache) : base(db, cache) { }

    public async Task<IReadOnlyList<VideoDto>> GetPublishedAsync() =>
        await _db.Videos.AsNoTracking().Where(x => x.IsPublished && x.IsActive)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new VideoDto(x.Id, x.Title, x.Description, x.Source, x.VideoUrl, x.YouTubeId, x.ThumbnailUrl, x.Category, x.IsPublished))
            .ToListAsync();

    public async Task<IReadOnlyList<VideoDto>> GetAllAsync() =>
        await _db.Videos.AsNoTracking().Where(x => x.IsActive)
            .Select(x => new VideoDto(x.Id, x.Title, x.Description, x.Source, x.VideoUrl, x.YouTubeId, x.ThumbnailUrl, x.Category, x.IsPublished))
            .ToListAsync();

    public async Task<VideoItem> CreateAsync(CreateVideoRequest request)
    {
        var video = new VideoItem
        {
            Title = request.Title, Description = request.Description, Source = request.Source,
            VideoUrl = request.VideoUrl, YouTubeId = request.YouTubeId, ThumbnailUrl = request.ThumbnailUrl,
            Category = request.Category, IsPublished = request.IsPublished
        };
        _db.Videos.Add(video);
        await _db.SaveChangesAsync();
        return video;
    }

    public async Task<VideoItem?> UpdateAsync(int id, CreateVideoRequest request)
    {
        var video = await _db.Videos.FindAsync(id);
        if (video == null) return null;
        video.Title = request.Title; video.Description = request.Description; video.Source = request.Source;
        video.VideoUrl = request.VideoUrl; video.YouTubeId = request.YouTubeId; video.ThumbnailUrl = request.ThumbnailUrl;
        video.Category = request.Category; video.IsPublished = request.IsPublished; video.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return video;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var video = await _db.Videos.FindAsync(id);
        if (video == null) return false;
        video.IsActive = false;
        await _db.SaveChangesAsync();
        return true;
    }
}

public class PublicationService : ContentServices, IPublicationService
{
    public PublicationService(ApplicationDbContext db, ICacheService cache) : base(db, cache) { }

    public async Task<IReadOnlyList<PublicationDto>> GetPublishedAsync(PublicationType? type = null)
    {
        var query = _db.Publications.AsNoTracking().Where(x => x.IsPublished && x.IsActive);
        if (type.HasValue) query = query.Where(x => x.Type == type);
        return await query.OrderByDescending(x => x.PublishedDate)
            .Select(x => new PublicationDto(x.Id, x.Title, x.Type, x.Edition, x.Description, x.PdfUrl, x.CoverImageUrl, x.PublishedDate, x.IsPublished))
            .ToListAsync();
    }

    public async Task<IReadOnlyList<PublicationDto>> GetAllAsync() =>
        await _db.Publications.AsNoTracking().Where(x => x.IsActive)
            .Select(x => new PublicationDto(x.Id, x.Title, x.Type, x.Edition, x.Description, x.PdfUrl, x.CoverImageUrl, x.PublishedDate, x.IsPublished))
            .ToListAsync();

    public async Task<Publication> CreateAsync(CreatePublicationRequest request)
    {
        var pub = new Publication
        {
            Title = request.Title, Type = request.Type, Edition = request.Edition, Description = request.Description,
            PdfUrl = request.PdfUrl, CoverImageUrl = request.CoverImageUrl, PublishedDate = request.PublishedDate, IsPublished = request.IsPublished
        };
        _db.Publications.Add(pub);
        await _db.SaveChangesAsync();
        return pub;
    }

    public async Task<Publication?> UpdateAsync(int id, CreatePublicationRequest request)
    {
        var pub = await _db.Publications.FindAsync(id);
        if (pub == null) return null;
        pub.Title = request.Title; pub.Type = request.Type; pub.Edition = request.Edition;
        pub.Description = request.Description; pub.PdfUrl = request.PdfUrl; pub.CoverImageUrl = request.CoverImageUrl;
        pub.PublishedDate = request.PublishedDate; pub.IsPublished = request.IsPublished; pub.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return pub;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var pub = await _db.Publications.FindAsync(id);
        if (pub == null) return false;
        pub.IsActive = false;
        await _db.SaveChangesAsync();
        return true;
    }
}

public class OfficeBearerService : ContentServices, IOfficeBearerService
{
    public OfficeBearerService(ApplicationDbContext db, ICacheService cache) : base(db, cache) { }

    public async Task<IReadOnlyList<OfficeBearerDto>> GetAllAsync() =>
        await _db.OfficeBearers.AsNoTracking().Where(x => x.IsActive)
            .OrderBy(x => x.Role).ThenBy(x => x.SortOrder)
            .Select(x => new OfficeBearerDto(x.Id, x.Name, x.Role, x.Designation, x.District, x.Phone, x.Email, x.PhotoUrl, x.SortOrder, x.Bio))
            .ToListAsync();

    public async Task<OfficeBearer> CreateAsync(CreateOfficeBearerRequest request)
    {
        var bearer = new OfficeBearer
        {
            Name = request.Name, Role = request.Role, Designation = request.Designation, District = request.District,
            Phone = request.Phone, Email = request.Email, PhotoUrl = request.PhotoUrl, SortOrder = request.SortOrder, Bio = request.Bio
        };
        _db.OfficeBearers.Add(bearer);
        await _db.SaveChangesAsync();
        return bearer;
    }

    public async Task<OfficeBearer?> UpdateAsync(int id, CreateOfficeBearerRequest request)
    {
        var bearer = await _db.OfficeBearers.FindAsync(id);
        if (bearer == null) return null;
        bearer.Name = request.Name; bearer.Role = request.Role; bearer.Designation = request.Designation;
        bearer.District = request.District; bearer.Phone = request.Phone; bearer.Email = request.Email;
        bearer.PhotoUrl = request.PhotoUrl; bearer.SortOrder = request.SortOrder; bearer.Bio = request.Bio;
        bearer.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return bearer;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var bearer = await _db.OfficeBearers.FindAsync(id);
        if (bearer == null) return false;
        bearer.IsActive = false;
        await _db.SaveChangesAsync();
        return true;
    }
}
