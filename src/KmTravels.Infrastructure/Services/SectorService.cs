using Microsoft.EntityFrameworkCore;
using SnlEngineering.Core.DTOs;
using SnlEngineering.Core.Entities;
using SnlEngineering.Core.Interfaces;
using SnlEngineering.Infrastructure.Data;

namespace SnlEngineering.Infrastructure.Services;

public class SectorService : ContentServices, ISectorService
{
    public SectorService(ApplicationDbContext db, ICacheService cache) : base(db, cache) { }

    public async Task<IReadOnlyList<SectorDto>> GetPublishedAsync(int? limit = null)
    {
        var cacheKey = $"sectors:pub:{limit}";
        var cached = await _cache.GetAsync<IReadOnlyList<SectorDto>>(cacheKey);
        if (cached != null) return cached;

        IQueryable<Sector> query = _db.Sectors.AsNoTracking()
            .Where(x => x.IsActive && x.IsPublished)
            .OrderBy(x => x.SortOrder);

        if (limit.HasValue)
            query = query.Take(limit.Value);

        var items = await query
            .Select(x => new SectorDto(x.Id, x.Title, x.Subtitle, x.Slug, x.CoverImageUrl, x.SortOrder, x.IsFeatured, x.IsPublished, x.Images.Count))
            .ToListAsync();

        await _cache.SetAsync(cacheKey, items);
        return items;
    }

    public async Task<SectorDetailDto?> GetBySlugAsync(string slug)
    {
        var sector = await _db.Sectors.AsNoTracking()
            .Include(x => x.Images.Where(i => i.IsActive))
            .FirstOrDefaultAsync(x => x.Slug == slug && x.IsPublished && x.IsActive);

        if (sector == null) return null;

        await _db.Sectors.Where(x => x.Id == sector.Id)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.ViewCount, x => x.ViewCount + 1));

        return MapDetail(sector, sector.ViewCount + 1);
    }

    public async Task<IReadOnlyList<SectorDto>> GetAllAsync() =>
        await _db.Sectors.AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.SortOrder)
            .Select(x => new SectorDto(x.Id, x.Title, x.Subtitle, x.Slug, x.CoverImageUrl, x.SortOrder, x.IsFeatured, x.IsPublished, x.Images.Count))
            .ToListAsync();

    public async Task<SectorDetailDto?> GetByIdAsync(int id)
    {
        var sector = await _db.Sectors.AsNoTracking()
            .Include(x => x.Images.Where(i => i.IsActive))
            .FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
        return sector == null ? null : MapDetail(sector, sector.ViewCount);
    }

    public async Task<SectorDetailDto?> CreateAsync(CreateSectorRequest request)
    {
        var slug = string.IsNullOrWhiteSpace(request.Slug) ? GenerateSlug(request.Title) : GenerateSlug(request.Slug);
        var sector = new Sector
        {
            Title = request.Title,
            Subtitle = request.Subtitle,
            Slug = slug,
            Content = request.Content,
            CoverImageUrl = request.CoverImageUrl,
            MetaTitle = request.MetaTitle,
            MetaDescription = request.MetaDescription,
            MetaKeywords = request.MetaKeywords,
            SortOrder = request.SortOrder,
            IsPublished = request.IsPublished,
            IsFeatured = request.IsFeatured
        };

        foreach (var img in request.Images.OrderBy(x => x.SortOrder))
        {
            sector.Images.Add(new SectorImage
            {
                Title = img.Title,
                ImageUrl = img.ImageUrl,
                Caption = img.Caption,
                SortOrder = img.SortOrder
            });
        }

        _db.Sectors.Add(sector);
        await _db.SaveChangesAsync();
        await _cache.RemoveByPrefixAsync("sectors:");
        return await GetByIdAsync(sector.Id);
    }

    public async Task<SectorDetailDto?> UpdateAsync(int id, CreateSectorRequest request)
    {
        var sector = await _db.Sectors
            .Include(x => x.Images)
            .FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
        if (sector == null) return null;

        sector.Title = request.Title;
        sector.Subtitle = request.Subtitle;
        sector.Slug = string.IsNullOrWhiteSpace(request.Slug) ? GenerateSlug(request.Title) : GenerateSlug(request.Slug);
        sector.Content = request.Content;
        sector.CoverImageUrl = request.CoverImageUrl;
        sector.MetaTitle = request.MetaTitle;
        sector.MetaDescription = request.MetaDescription;
        sector.MetaKeywords = request.MetaKeywords;
        sector.SortOrder = request.SortOrder;
        sector.IsPublished = request.IsPublished;
        sector.IsFeatured = request.IsFeatured;
        sector.UpdatedAt = DateTime.UtcNow;

        _db.SectorImages.RemoveRange(sector.Images);
        sector.Images.Clear();

        foreach (var img in request.Images.OrderBy(x => x.SortOrder))
        {
            sector.Images.Add(new SectorImage
            {
                Title = img.Title,
                ImageUrl = img.ImageUrl,
                Caption = img.Caption,
                SortOrder = img.SortOrder
            });
        }

        await _db.SaveChangesAsync();
        await _cache.RemoveByPrefixAsync("sectors:");
        return await GetByIdAsync(sector.Id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var sector = await _db.Sectors.FindAsync(id);
        if (sector == null) return false;
        sector.IsActive = false;
        await _db.SaveChangesAsync();
        await _cache.RemoveByPrefixAsync("sectors:");
        return true;
    }

    public async Task<bool> TogglePublishAsync(int id)
    {
        var sector = await _db.Sectors.FindAsync(id);
        if (sector == null) return false;
        sector.IsPublished = !sector.IsPublished;
        sector.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        await _cache.RemoveByPrefixAsync("sectors:");
        return true;
    }

    private static SectorDetailDto MapDetail(Sector sector, int viewCount) =>
        new(sector.Id, sector.Title, sector.Subtitle, sector.Slug, sector.Content, sector.CoverImageUrl,
            sector.MetaTitle, sector.MetaDescription, sector.MetaKeywords, sector.SortOrder,
            sector.IsFeatured, sector.IsPublished, viewCount,
            sector.Images.OrderBy(x => x.SortOrder)
                .Select(x => new SectorImageDto(x.Id, x.SectorId, x.Title, x.ImageUrl, x.Caption, x.SortOrder))
                .ToList());
}
