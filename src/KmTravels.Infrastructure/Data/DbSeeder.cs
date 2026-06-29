using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using KmTravels.Core.Entities;
using KmTravels.Core.Enums;
using KmTravels.Core.Interfaces;

namespace KmTravels.Infrastructure.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var cache = scope.ServiceProvider.GetRequiredService<ICacheService>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        await db.Database.MigrateAsync();

        foreach (var role in UserRoles.All)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        if (await userManager.FindByEmailAsync("admin@KmTravels.com") == null)
        {
            var admin = new ApplicationUser
            {
                UserName = "admin@KmTravels.com",
                Email = "admin@KmTravels.com",
                FullName = "Super Administrator",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(admin, "Admin@123");
            await userManager.AddToRoleAsync(admin, UserRoles.SuperAdmin);
        }

        if (!await db.SiteSettings.AnyAsync())
        {
            db.SiteSettings.AddRange(
                new SiteSetting { Key = "AssociationName", Value = "SNL Engineering" },
                new SiteSetting { Key = "Tagline", Value = "Concept-to-Commissioning EPCM Engineering Consultants" },
                new SiteSetting { Key = "Address", Value = "Chennai, Tamil Nadu, India" },
                new SiteSetting { Key = "Phone", Value = "+91 44 4598 1200" },
                new SiteSetting { Key = "Email", Value = "info@KmTravels.com" },
                new SiteSetting { Key = "AboutContent", Value = "SNL Engineering is a premier engineering consultancy offering end-to-end EPCM (Engineering, Procurement and Construction Management) services. We guide industrial projects from initial concept through final commissioning across power, sugar, ethanol, chemical, and process industries." },
                new SiteSetting { Key = "Vision", Value = "To be among the top engineering consultants in India, delivering innovative, energy-efficient, and environmentally friendly engineering solutions under one roof." },
                new SiteSetting { Key = "Mission", Value = "To provide total engineering solutions in all disciplines for complete project implementation — from Conceptual System Design and Techno-Economic Feasibility Reports to Detailed Engineering, Project Management, and Commissioning assistance." },
                new SiteSetting { Key = "Objectives", Value = "Deliver end-to-end EPC and EPCM project solutions\nProvide multi-disciplinary detailed engineering services\nEnsure energy efficiency and environmental compliance\nSupport clients with third-party inspection and construction supervision\nMaintain ISO 9001:2015 certified quality standards" }
            );
        }

        if (!await db.BannerSlides.AnyAsync())
        {
            db.BannerSlides.AddRange(
                new BannerSlide { Title = "Top Engineering Consultants in India", Subtitle = "Concept-to-Commissioning EPCM Services", ImageUrl = "/images/banner1.jpg", LinkUrl = "/about", SortOrder = 1 },
                new BannerSlide { Title = "Engineering Excellence", Subtitle = "Multi-disciplinary EPCM Solutions", ImageUrl = "/images/banner2.jpg", LinkUrl = "/about", SortOrder = 2 },
                new BannerSlide { Title = "Power, Sugar, Ethanol & Process Industries", Subtitle = "Complete Engineering Solutions Under One Roof", ImageUrl = "/images/banner3.jpg", LinkUrl = "/contact", SortOrder = 3 }
            );
        }

        await SeedMenuItemsAsync(db);

        if (!await db.OfficeBearers.AnyAsync())
        {
            db.OfficeBearers.AddRange(
                new OfficeBearer { Name = "Managing Director", Role = OfficeBearerRole.President, Designation = "Managing Director", Phone = "+91 44 4598 1200", SortOrder = 1 },
                new OfficeBearer { Name = "Technical Director", Role = OfficeBearerRole.Secretary, Designation = "Technical Director", SortOrder = 2 },
                new OfficeBearer { Name = "Project Director", Role = OfficeBearerRole.Treasurer, Designation = "Project Director", SortOrder = 3 }
            );
        }

        if (!await db.NewsArticles.AnyAsync())
        {
            db.NewsArticles.AddRange(
                new NewsArticle { Title = "SNL Engineering Expands EPCM Services", Slug = "snl-engineering-epcm-expansion", Summary = "SNL Engineering continues to grow its industrial engineering consulting capabilities.", Content = "<p>SNL Engineering is expanding its Concept-to-Commissioning EPCM services across power, sugar, ethanol, and process industries.</p>", Category = NewsCategory.LatestNews, IsPublished = true, PublishedAt = DateTime.UtcNow, Author = "SNL Engineering" },
                new NewsArticle { Title = "Top CBG Plant Consultants in India", Slug = "cbg-plant-consultants-india", Summary = "SNL Engineering delivers end-to-end Compressed Biogas plant solutions with turnkey EPCM services.", Content = "<p>With India's growing focus on renewable energy, SNL Engineering offers comprehensive CBG plant design, procurement, and commissioning services.</p>", Category = NewsCategory.IndustryUpdate, IsPublished = true, PublishedAt = DateTime.UtcNow.AddDays(-2), Author = "Engineering Desk" },
                new NewsArticle { Title = "EPCM Services for Power & Co-generation Projects", Slug = "epcm-power-cogeneration", Summary = "Complete engineering solutions for power plants, co-generation, and solar installations.", Content = "<p>SNL Engineering provides multi-disciplinary engineering and construction management for power sector projects including co-generation plants, solar installations, and water treatment facilities.</p>", Category = NewsCategory.PressRelease, IsPublished = true, PublishedAt = DateTime.UtcNow.AddDays(-5), Author = "Project Team" }
            );
        }

        if (!await db.GalleryAlbums.AnyAsync())
        {
            const string img1 = "https://images.unsplash.com/photo-1581091226825-a6a2a5aee158?auto=format&fit=crop&w=1920&q=80";
            const string img2 = "https://images.unsplash.com/photo-1504328345606-18bbc8c9d7d1?auto=format&fit=crop&w=1920&q=80";
            const string img3 = "https://images.unsplash.com/photo-1581092160562-40aa08e78837?auto=format&fit=crop&w=1920&q=80";
            const string img4 = "https://images.unsplash.com/photo-1473341304170-971dccb5ac1e?auto=format&fit=crop&w=1920&q=80";
            const string img5 = "https://images.unsplash.com/photo-1513828583688-c52646db42da?auto=format&fit=crop&w=1920&q=80";

            var slideshowAlbum = new GalleryAlbum
            {
                Title = "Engineering Excellence",
                Description = "Industrial and engineering project imagery for the home page slideshow.",
                Category = "Slideshow",
                CoverImageUrl = img1,
                Images =
                [
                    new GalleryImage { Title = "Industrial Engineering", Caption = "Multi-disciplinary engineering solutions", ImageUrl = img1, SortOrder = 1 },
                    new GalleryImage { Title = "Process Industries", Caption = "Power, sugar, ethanol and chemical plants", ImageUrl = img2, SortOrder = 2 },
                    new GalleryImage { Title = "Detailed Design", Caption = "Concept-to-commissioning EPCM services", ImageUrl = img3, SortOrder = 3 },
                    new GalleryImage { Title = "Renewable Energy", Caption = "Solar and co-generation projects", ImageUrl = img4, SortOrder = 4 },
                    new GalleryImage { Title = "Construction Management", Caption = "End-to-end project delivery", ImageUrl = img5, SortOrder = 5 }
                ]
            };

            var projectsAlbum = new GalleryAlbum
            {
                Title = "Project Portfolio",
                Description = "Selected engineering and construction projects.",
                Category = "Projects",
                CoverImageUrl = img2,
                EventDate = DateTime.UtcNow.AddMonths(-1),
                Images =
                [
                    new GalleryImage { Title = "Industrial Plant", ImageUrl = img2, SortOrder = 1 },
                    new GalleryImage { Title = "Engineering Workspace", ImageUrl = img3, SortOrder = 2 },
                    new GalleryImage { Title = "Solar Installation", ImageUrl = img4, SortOrder = 3 }
                ]
            };

            db.GalleryAlbums.AddRange(slideshowAlbum, projectsAlbum);
        }

        await SeedServicesAsync(db);
        await SeedSectorsAsync(db);

        await db.SaveChangesAsync();
        await cache.RemoveByPrefixAsync("services:");
        await cache.RemoveByPrefixAsync("sectors:");
        await cache.RemoveAsync("menu:tree");
    }

    private static async Task SeedMenuItemsAsync(ApplicationDbContext db)
    {
        var publicationsMenu = await db.MenuItems.FirstOrDefaultAsync(m => m.Url == "/publications");
        if (publicationsMenu != null)
        {
            publicationsMenu.Title = "Sectors";
            publicationsMenu.Url = "/sectors";
        }

        var officeBearersMenu = await db.MenuItems.FirstOrDefaultAsync(m => m.Url == "/office-bearers");
        if (officeBearersMenu != null)
        {
            officeBearersMenu.Title = "Leadership";
            officeBearersMenu.Url = "/leadership";
        }

        var membershipMenu = await db.MenuItems.FirstOrDefaultAsync(m => m.Url == "/membership");
        if (membershipMenu != null)
        {
            membershipMenu.Title = "Project Enquiry";
            membershipMenu.Url = "/project-inquiry";
        }

        var galleryMenu = await db.MenuItems.FirstOrDefaultAsync(m => m.Url == "/gallery");
        if (galleryMenu != null)
        {
            galleryMenu.Title = "Projects";
            galleryMenu.Url = "/projects";
        }

        var defaults = new (string Title, string Url, int SortOrder)[]
        {
            ("Home", "/", 1),
            ("About Us", "/about", 2),
            ("Services", "/services", 3),
            ("Leadership", "/leadership", 4),
            ("News", "/news", 5),
            ("Projects", "/projects", 6),
            ("Videos", "/videos", 7),
            ("Sectors", "/sectors", 8),
            ("Project Enquiry", "/project-inquiry", 9),
            ("Contact", "/contact", 10)
        };

        var existingUrls = await db.MenuItems
            .Where(x => x.IsActive)
            .Select(x => x.Url)
            .ToListAsync();

        var urlSet = existingUrls.ToHashSet(StringComparer.OrdinalIgnoreCase);
        foreach (var item in defaults)
        {
            if (!urlSet.Contains(item.Url))
            {
                db.MenuItems.Add(new MenuItem
                {
                    Title = item.Title,
                    Url = item.Url,
                    SortOrder = item.SortOrder
                });
            }
        }
    }

    private static async Task SeedServicesAsync(ApplicationDbContext db)
    {
        var seeds = CreateSeedServices().ToList();
        var existingSlugs = await db.ServiceOfferings
            .Select(x => x.Slug)
            .ToListAsync();

        var slugSet = existingSlugs.ToHashSet(StringComparer.OrdinalIgnoreCase);
        var missing = seeds.Where(s => !slugSet.Contains(s.Slug)).ToList();
        if (missing.Count > 0)
            db.ServiceOfferings.AddRange(missing);
    }

    private static IEnumerable<ServiceOffering> CreateSeedServices()
    {
        const string img1 = "https://images.unsplash.com/photo-1581091226825-a6a2a5aee158?auto=format&fit=crop&w=1200&q=80";
        const string img2 = "https://images.unsplash.com/photo-1504328345606-18bbc8c9d7d1?auto=format&fit=crop&w=1200&q=80";
        const string img3 = "https://images.unsplash.com/photo-1581092160562-40aa08e78837?auto=format&fit=crop&w=1200&q=80";
        const string img4 = "https://images.unsplash.com/photo-1473341304170-971dccb5ac1e?auto=format&fit=crop&w=1200&q=80";
        const string img5 = "https://images.unsplash.com/photo-1513828583688-c52646db42da?auto=format&fit=crop&w=1200&q=80";

        var seeds = new (string Title, string Subtitle, string Slug, int Rating, int Order, string Content, string MetaDesc, string[] Images)[]
        {
            ("EPCM", "Engineering, Procurement & Construction Management", "epcm", 10, 1,
                "<p>End-to-end EPCM services covering engineering design, procurement support, and construction management for industrial projects from concept through commissioning.</p>",
                "EPCM services — engineering, procurement and construction management for industrial projects.",
                [img1, img2]),
            ("Detailed Engineering", "Multi-disciplinary design & documentation", "detailed-engineering", 10, 2,
                "<p>Complete detailed engineering across mechanical, electrical, civil, and process disciplines with compliant drawings, specifications, and BOQs.</p>",
                "Detailed engineering services for power, process, and industrial plant projects.",
                [img3, img2]),
            ("Project Management Services", "Schedule, cost & quality control", "project-management-services", 9, 3,
                "<p>Professional project management including planning, monitoring, risk management, and stakeholder coordination for complex EPC/EPCM projects.</p>",
                "Project management services for engineering and construction projects in India.",
                [img5, img3]),
            ("Techno-Economic Feasibility Study", "TEFR & investment analysis", "techno-economic-feasibility-study", 9, 4,
                "<p>Techno-economic feasibility reports evaluating technical viability, capital costs, operating economics, and ROI for greenfield and expansion projects.</p>",
                "Techno-economic feasibility study (TEFR) for industrial and power projects.",
                [img4, img1]),
            ("Construction Supervision", "On-site quality & progress monitoring", "construction-supervision", 8, 5,
                "<p>Construction supervision and site management ensuring workmanship, safety compliance, and adherence to engineering specifications.</p>",
                "Construction supervision services for industrial plant and infrastructure projects.",
                [img5, img2]),
            ("Commissioning Services", "Start-up & performance testing", "commissioning-services", 8, 6,
                "<p>Pre-commissioning checks, system testing, trial runs, and handover support to achieve reliable plant start-up and performance guarantees.</p>",
                "Commissioning assistance and start-up services for process and power plants.",
                [img3, img4]),
            ("Procurement Services", "Vendor evaluation & expediting", "procurement-services", 8, 7,
                "<p>Procurement engineering, vendor pre-qualification, bid evaluation, purchase order management, and material expediting services.</p>",
                "Procurement services for EPC and EPCM industrial projects.",
                [img2, img5]),
            ("Third-Party Inspection Services", "Independent QA/QC verification", "third-party-inspection-services", 7, 8,
                "<p>Independent third-party inspection and quality assurance for equipment, fabrication, and installation activities.</p>",
                "Third-party inspection services for engineering and construction projects.",
                [img1, img3]),
            ("Boiler Design", "Steam generation system engineering", "boiler-design", 7, 9,
                "<p>Boiler system design, performance calculations, layout engineering, and compliance with applicable codes and standards.</p>",
                "Boiler design engineering services for co-generation and process industries.",
                [img2, img4]),
            ("Energy Audits", "Efficiency assessment & optimization", "energy-audits", 7, 10,
                "<p>Comprehensive energy audits identifying conservation opportunities, load optimization, and cost reduction across plant operations.</p>",
                "Energy audit services for industrial plants and co-generation facilities.",
                [img4, img5])
        };

        return seeds.Select(s => new ServiceOffering
        {
            Title = s.Title,
            Subtitle = s.Subtitle,
            Slug = s.Slug,
            Content = s.Content,
            CoverImageUrl = s.Images[0],
            MetaTitle = $"{s.Title} | SNL Engineering",
            MetaDescription = s.MetaDesc,
            MetaKeywords = $"{s.Title.ToLower()}, epcm, engineering consultants, snl engineering",
            SortOrder = s.Order,
            DemandRating = s.Rating,
            IsPublished = true,
            IsFeatured = true,
            Images = s.Images.Select((url, i) => new ServiceImage
            {
                Title = $"{s.Title} — Image {i + 1}",
                ImageUrl = url,
                SortOrder = i + 1
            }).ToList()
        });
    }

    private static async Task SeedSectorsAsync(ApplicationDbContext db)
    {
        var seeds = CreateSeedSectors().ToList();
        var existingSlugs = await db.Sectors.Select(x => x.Slug).ToListAsync();
        var slugSet = existingSlugs.ToHashSet(StringComparer.OrdinalIgnoreCase);
        var missing = seeds.Where(s => !slugSet.Contains(s.Slug)).ToList();
        if (missing.Count > 0)
            db.Sectors.AddRange(missing);
    }

    private static IEnumerable<Sector> CreateSeedSectors()
    {
        const string imgPower = "https://images.unsplash.com/photo-1473341304170-971dccb5ac1e?auto=format&fit=crop&w=1200&q=80";
        const string imgOilGas = "https://images.unsplash.com/photo-1581091226825-a6a2a5aee158?auto=format&fit=crop&w=1200&q=80";
        const string imgPetrochemical = "https://images.unsplash.com/photo-1581092160562-40aa08e78837?auto=format&fit=crop&w=1200&q=80";
        const string imgRefinery = "https://images.unsplash.com/photo-1504328345606-18bbc8c9d7d1?auto=format&fit=crop&w=1200&q=80";
        const string imgWater = "https://images.unsplash.com/photo-1548839140-29a7492991a9?auto=format&fit=crop&w=1200&q=80";
        const string imgChemical = "https://images.unsplash.com/photo-1532187863486-abf9db6281e1?auto=format&fit=crop&w=1200&q=80";
        const string imgPharma = "https://images.unsplash.com/photo-1582719478250-c89cae4dc85b?auto=format&fit=crop&w=1200&q=80";
        const string imgPaper = "https://images.unsplash.com/photo-1565008576549-57569a49371d?auto=format&fit=crop&w=1200&q=80";
        const string imgBuildings = "https://images.unsplash.com/photo-1486406146926-c627a92ad1ab?auto=format&fit=crop&w=1200&q=80";

        var seeds = new (string Title, string Subtitle, string Slug, int Order, string Content, string MetaDesc, string Cover, string[] Images)[]
        {
            ("Power", "Generation, co-generation & renewable energy", "power", 1,
                "<p>Engineering solutions for thermal power plants, co-generation systems, solar installations, and grid-connected renewable energy projects.</p>",
                "Power sector engineering — generation, co-generation and renewable energy projects.",
                imgPower, [imgPower, imgRefinery]),
            ("Oil & Gas", "Upstream, midstream & downstream facilities", "oil-gas", 2,
                "<p>EPCM and detailed engineering for oil and gas processing facilities, pipelines, storage terminals, and offshore support infrastructure.</p>",
                "Oil and gas sector engineering consultancy for upstream and downstream projects.",
                imgOilGas, [imgOilGas, imgPetrochemical]),
            ("Petrochemical", "Process plants & polymer complexes", "petrochemical", 3,
                "<p>Design and engineering for petrochemical complexes, polymer plants, and specialty chemical production facilities.</p>",
                "Petrochemical plant engineering and EPCM services.",
                imgPetrochemical, [imgPetrochemical, imgChemical]),
            ("Refinery", "Crude processing & product upgrading", "refinery", 4,
                "<p>Refinery engineering including crude distillation, hydroprocessing, catalytic units, and utilities integration for petroleum refineries.</p>",
                "Refinery engineering services for crude processing and upgrading units.",
                imgRefinery, [imgRefinery, imgOilGas]),
            ("Water Treatment", "Industrial & municipal water systems", "water-treatment", 5,
                "<p>Water treatment plant design, effluent treatment, zero liquid discharge systems, and desalination project engineering.</p>",
                "Water treatment and effluent management engineering services.",
                imgWater, [imgWater, imgPower]),
            ("Chemical", "Batch & continuous process plants", "chemical", 6,
                "<p>Multi-disciplinary engineering for chemical manufacturing plants, reactor systems, and hazardous area compliance.</p>",
                "Chemical plant detailed engineering and project management.",
                imgChemical, [imgChemical, imgPetrochemical]),
            ("Pharmaceutical", "GMP-compliant facility design", "pharmaceutical", 7,
                "<p>Pharmaceutical and API facility engineering with GMP compliance, cleanroom design, and validation support.</p>",
                "Pharmaceutical facility engineering and GMP-compliant design.",
                imgPharma, [imgPharma, imgChemical]),
            ("Paper & Pulp", "Mill engineering & recovery systems", "paper-pulp", 8,
                "<p>Engineering for pulp mills, paper machines, chemical recovery, and effluent treatment in the pulp and paper industry.</p>",
                "Paper and pulp mill engineering and EPCM services.",
                imgPaper, [imgPaper, imgWater]),
            ("Buildings", "Industrial & commercial structures", "buildings", 9,
                "<p>Structural and MEP engineering for industrial buildings, administrative blocks, warehouses, and multi-storey commercial facilities.</p>",
                "Industrial and commercial building engineering services.",
                imgBuildings, [imgBuildings, imgPower])
        };

        return seeds.Select(s => new Sector
        {
            Title = s.Title,
            Subtitle = s.Subtitle,
            Slug = s.Slug,
            Content = s.Content,
            CoverImageUrl = s.Cover,
            MetaTitle = $"{s.Title} Sector | SNL Engineering",
            MetaDescription = s.MetaDesc,
            MetaKeywords = $"{s.Title.ToLower()}, industrial sector, engineering consultants, snl engineering",
            SortOrder = s.Order,
            IsPublished = true,
            IsFeatured = true,
            Images = s.Images.Select((url, i) => new SectorImage
            {
                Title = $"{s.Title} — Image {i + 1}",
                ImageUrl = url,
                SortOrder = i + 1
            }).ToList()
        });
    }
}
