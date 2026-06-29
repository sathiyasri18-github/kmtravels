using SnlEngineering.Core.Enums;

namespace SnlEngineering.Core.Entities;

public class NewsArticle : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public NewsCategory Category { get; set; }
    public bool IsPublished { get; set; }
    public DateTime? PublishedAt { get; set; }
    public string? Author { get; set; }
    public int ViewCount { get; set; }
}

public class GalleryAlbum : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? CoverImageUrl { get; set; }
    public string? Category { get; set; }
    public DateTime? EventDate { get; set; }
    public ICollection<GalleryImage> Images { get; set; } = [];
}

public class GalleryImage : BaseEntity
{
    public int AlbumId { get; set; }
    public GalleryAlbum Album { get; set; } = null!;
    public string Title { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string? Caption { get; set; }
    public int SortOrder { get; set; }
}

public class VideoItem : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public VideoSource Source { get; set; }
    public string? VideoUrl { get; set; }
    public string? YouTubeId { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string? Category { get; set; }
    public bool IsPublished { get; set; }
}

public class Publication : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public PublicationType Type { get; set; }
    public string? Edition { get; set; }
    public string? Description { get; set; }
    public string PdfUrl { get; set; } = string.Empty;
    public string? CoverImageUrl { get; set; }
    public DateTime? PublishedDate { get; set; }
    public bool IsPublished { get; set; }
}

public class OfficeBearer : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public OfficeBearerRole Role { get; set; }
    public string? Designation { get; set; }
    public string? District { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? PhotoUrl { get; set; }
    public int SortOrder { get; set; }
    public string? Bio { get; set; }
}

public class Advertisement : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string? LinkUrl { get; set; }
    public AdvertisementPosition Position { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int SortOrder { get; set; }
}

public class BannerSlide : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Subtitle { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? LinkUrl { get; set; }
    public int SortOrder { get; set; }
}

public class MenuItem : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Url { get; set; }
    public int? ParentId { get; set; }
    public MenuItem? Parent { get; set; }
    public ICollection<MenuItem> Children { get; set; } = [];
    public int SortOrder { get; set; }
    public bool OpenInNewTab { get; set; }
}

public class DynamicPage : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
    public bool IsPublished { get; set; }
}

public class ContactInquiry : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public bool IsReplied { get; set; }
}

public class SiteSetting : BaseEntity
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class VisitorStat
{
    public int Id { get; set; }
    public long TotalVisitors { get; set; }
    public long TotalPageViews { get; set; }
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}

public class MemberRegistration : BaseEntity
{
    public MemberType MemberType { get; set; }
    public MemberStatus Status { get; set; } = MemberStatus.Pending;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? District { get; set; }
    public string? State { get; set; }
    public string? CompanyName { get; set; }
    public string? LicenseNumber { get; set; }
    public string? DocumentUrl { get; set; }
    public DateTime? MembershipExpiry { get; set; }
    public string? MembershipNumber { get; set; }
    public string? Notes { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTime? ApprovedAt { get; set; }
}

public class EventItem : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Location { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsPublished { get; set; }
}

public class ServiceOffering : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Subtitle { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? CoverImageUrl { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
    public int SortOrder { get; set; }
    public int DemandRating { get; set; }
    public bool IsPublished { get; set; }
    public bool IsFeatured { get; set; }
    public int ViewCount { get; set; }
    public ICollection<ServiceImage> Images { get; set; } = [];
}

public class ServiceImage : BaseEntity
{
    public int ServiceId { get; set; }
    public ServiceOffering Service { get; set; } = null!;
    public string Title { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string? Caption { get; set; }
    public int SortOrder { get; set; }
}

public class Sector : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Subtitle { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? CoverImageUrl { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
    public int SortOrder { get; set; }
    public bool IsPublished { get; set; }
    public bool IsFeatured { get; set; }
    public int ViewCount { get; set; }
    public ICollection<SectorImage> Images { get; set; } = [];
}

public class SectorImage : BaseEntity
{
    public int SectorId { get; set; }
    public Sector Sector { get; set; } = null!;
    public string Title { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string? Caption { get; set; }
    public int SortOrder { get; set; }
}
