using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SnlEngineering.Core.Entities;

namespace SnlEngineering.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<NewsArticle> NewsArticles => Set<NewsArticle>();
    public DbSet<GalleryAlbum> GalleryAlbums => Set<GalleryAlbum>();
    public DbSet<GalleryImage> GalleryImages => Set<GalleryImage>();
    public DbSet<VideoItem> Videos => Set<VideoItem>();
    public DbSet<Publication> Publications => Set<Publication>();
    public DbSet<OfficeBearer> OfficeBearers => Set<OfficeBearer>();
    public DbSet<Advertisement> Advertisements => Set<Advertisement>();
    public DbSet<BannerSlide> BannerSlides => Set<BannerSlide>();
    public DbSet<MenuItem> MenuItems => Set<MenuItem>();
    public DbSet<DynamicPage> DynamicPages => Set<DynamicPage>();
    public DbSet<ContactInquiry> ContactInquiries => Set<ContactInquiry>();
    public DbSet<SiteSetting> SiteSettings => Set<SiteSetting>();
    public DbSet<VisitorStat> VisitorStats => Set<VisitorStat>();
    public DbSet<MemberRegistration> MemberRegistrations => Set<MemberRegistration>();
    public DbSet<EventItem> Events => Set<EventItem>();
    public DbSet<ServiceOffering> ServiceOfferings => Set<ServiceOffering>();
    public DbSet<ServiceImage> ServiceImages => Set<ServiceImage>();
    public DbSet<Sector> Sectors => Set<Sector>();
    public DbSet<SectorImage> SectorImages => Set<SectorImage>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<NewsArticle>(e =>
        {
            e.HasIndex(x => x.Slug).IsUnique();
            e.Property(x => x.Title).HasMaxLength(500);
        });

        builder.Entity<GalleryAlbum>(e => e.Property(x => x.Title).HasMaxLength(300));
        builder.Entity<GalleryImage>(e => e.HasOne(x => x.Album).WithMany(x => x.Images).HasForeignKey(x => x.AlbumId).OnDelete(DeleteBehavior.Cascade));

        builder.Entity<MenuItem>(e =>
        {
            e.HasOne(x => x.Parent).WithMany(x => x.Children).HasForeignKey(x => x.ParentId).OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<DynamicPage>(e => e.HasIndex(x => x.Slug).IsUnique());

        builder.Entity<ServiceOffering>(e =>
        {
            e.HasIndex(x => x.Slug).IsUnique();
            e.Property(x => x.Title).HasMaxLength(300);
            e.Property(x => x.Subtitle).HasMaxLength(500);
        });
        builder.Entity<ServiceImage>(e =>
            e.HasOne(x => x.Service).WithMany(x => x.Images).HasForeignKey(x => x.ServiceId).OnDelete(DeleteBehavior.Cascade));

        builder.Entity<Sector>(e =>
        {
            e.HasIndex(x => x.Slug).IsUnique();
            e.Property(x => x.Title).HasMaxLength(300);
            e.Property(x => x.Subtitle).HasMaxLength(500);
        });
        builder.Entity<SectorImage>(e =>
            e.HasOne(x => x.Sector).WithMany(x => x.Images).HasForeignKey(x => x.SectorId).OnDelete(DeleteBehavior.Cascade));

        builder.Entity<SiteSetting>(e => e.HasIndex(x => x.Key).IsUnique());
        builder.Entity<VisitorStat>().HasData(new VisitorStat { Id = 1, TotalVisitors = 0, TotalPageViews = 0 });
    }
}
