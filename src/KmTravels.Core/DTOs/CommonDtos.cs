using KmTravels.Core.Enums;

namespace KmTravels.Core.DTOs;

public record PagedResult<T>(IReadOnlyList<T> Items, int TotalCount, int Page, int PageSize);

public record ApiResponse<T>(bool Success, T? Data, string? Message = null);

public record LoginRequest(string Email, string Password);
public record LoginResponse(string Token, string Email, string FullName, IList<string> Roles, DateTime ExpiresAt);

public record NewsDto(int Id, string Title, string Slug, string Summary, string? ImageUrl, NewsCategory Category, bool IsPublished, DateTime? PublishedAt, string? Author, int ViewCount);
public record NewsDetailDto(int Id, string Title, string Slug, string Summary, string Content, string? ImageUrl, NewsCategory Category, DateTime? PublishedAt, string? Author, int ViewCount);
public record CreateNewsRequest(string Title, string Summary, string Content, string? ImageUrl, NewsCategory Category, bool IsPublished, string? Author);

public record GalleryAlbumDto(int Id, string Title, string? Description, string? CoverImageUrl, string? Category, DateTime? EventDate, int ImageCount);
public record GalleryImageDto(int Id, int AlbumId, string Title, string ImageUrl, string? Caption, int SortOrder);
public record CreateAlbumRequest(string Title, string? Description, string? Category, DateTime? EventDate);

public record VideoDto(int Id, string Title, string? Description, VideoSource Source, string? VideoUrl, string? YouTubeId, string? ThumbnailUrl, string? Category, bool IsPublished);
public record CreateVideoRequest(string Title, string? Description, VideoSource Source, string? VideoUrl, string? YouTubeId, string? ThumbnailUrl, string? Category, bool IsPublished);

public record PublicationDto(int Id, string Title, PublicationType Type, string? Edition, string? Description, string PdfUrl, string? CoverImageUrl, DateTime? PublishedDate, bool IsPublished);
public record CreatePublicationRequest(string Title, PublicationType Type, string? Edition, string? Description, string PdfUrl, string? CoverImageUrl, DateTime? PublishedDate, bool IsPublished);

public record OfficeBearerDto(int Id, string Name, OfficeBearerRole Role, string? Designation, string? District, string? Phone, string? Email, string? PhotoUrl, int SortOrder, string? Bio);
public record CreateOfficeBearerRequest(string Name, OfficeBearerRole Role, string? Designation, string? District, string? Phone, string? Email, string? PhotoUrl, int SortOrder, string? Bio);

public record AdvertisementDto(int Id, string Title, string ImageUrl, string? LinkUrl, AdvertisementPosition Position, DateTime StartDate, DateTime EndDate, int SortOrder);
public record BannerSlideDto(int Id, string Title, string? Subtitle, string ImageUrl, string? LinkUrl, int SortOrder);

public record MenuItemDto(int Id, string Title, string? Url, int? ParentId, int SortOrder, bool OpenInNewTab, List<MenuItemDto> Children);
public record DynamicPageDto(int Id, string Title, string Slug, string Content, string? MetaDescription, bool IsPublished);

public record ContactInquiryDto(int Id, string Name, string Email, string? Phone, string Subject, string Message, bool IsRead, DateTime CreatedAt);
public record SubmitContactRequest(string Name, string Email, string? Phone, string Subject, string Message);

public record MemberRegistrationDto(int Id, MemberType MemberType, MemberStatus Status, string FullName, string Email, string Phone, string? District, string? CompanyName, string? MembershipNumber, DateTime? MembershipExpiry, DateTime CreatedAt);
public record SubmitMemberRequest(MemberType MemberType, string FullName, string Email, string Phone, string? Address, string? District, string? State, string? CompanyName, string? LicenseNumber, string? DocumentUrl);

public record ProjectInquiryDto(int Id, string FullName, string Email, string Phone, string? CompanyName, string? City, string? State, string? ProjectType, string? ProjectDetails, MemberStatus Status, string? Notes, DateTime CreatedAt);
public record CreateProjectInquiryRequest(string FullName, string Email, string Phone, string? CompanyName, string? City, string? State, string? ProjectType, string? ProjectDetails, MemberStatus Status, string? Notes);
public record UpdateProjectInquiryRequest(string FullName, string Email, string Phone, string? CompanyName, string? City, string? State, string? ProjectType, string? ProjectDetails, MemberStatus Status, string? Notes);

public record EventDto(int Id, string Title, string? Description, string? Location, DateTime StartDate, DateTime? EndDate, string? ImageUrl, bool IsPublished);

public record ServiceImageDto(int Id, int ServiceId, string Title, string ImageUrl, string? Caption, int SortOrder);
public record ServiceImageInput(string Title, string ImageUrl, string? Caption, int SortOrder);
public record ServiceDto(int Id, string Title, string? Subtitle, string Slug, string? CoverImageUrl, int SortOrder, int DemandRating, bool IsFeatured, bool IsPublished, int ImageCount);
public record ServiceDetailDto(int Id, string Title, string? Subtitle, string Slug, string Content, string? CoverImageUrl,
    string? MetaTitle, string? MetaDescription, string? MetaKeywords, int SortOrder, int DemandRating, bool IsFeatured, bool IsPublished,
    int ViewCount, IReadOnlyList<ServiceImageDto> Images);
public record CreateServiceRequest(string Title, string? Subtitle, string Content, string? CoverImageUrl,
    string? MetaTitle, string? MetaDescription, string? MetaKeywords, string? Slug,
    int SortOrder, int DemandRating, bool IsPublished, bool IsFeatured, IList<ServiceImageInput> Images);

public record SectorImageDto(int Id, int SectorId, string Title, string ImageUrl, string? Caption, int SortOrder);
public record SectorImageInput(string Title, string ImageUrl, string? Caption, int SortOrder);
public record SectorDto(int Id, string Title, string? Subtitle, string Slug, string? CoverImageUrl, int SortOrder, bool IsFeatured, bool IsPublished, int ImageCount);
public record SectorDetailDto(int Id, string Title, string? Subtitle, string Slug, string Content, string? CoverImageUrl,
    string? MetaTitle, string? MetaDescription, string? MetaKeywords, int SortOrder, bool IsFeatured, bool IsPublished,
    int ViewCount, IReadOnlyList<SectorImageDto> Images);
public record CreateSectorRequest(string Title, string? Subtitle, string Content, string? CoverImageUrl,
    string? MetaTitle, string? MetaDescription, string? MetaKeywords, string? Slug,
    int SortOrder, bool IsPublished, bool IsFeatured, IList<SectorImageInput> Images);

public record DashboardStatsDto(long TotalVisitors, long TotalPageViews, int TotalNews, int TotalProjectInquiries, int PendingProjectInquiries, int TotalInquiries, int UnreadInquiries);
public record VisitorStatsDto(long TotalVisitors, long TotalPageViews);

public record SiteSettingsDto(string AssociationName, string Tagline, string Address, string Phone, string Email, string? MapEmbedUrl, string? AboutContent, string? Vision, string? Mission, string? Objectives);
