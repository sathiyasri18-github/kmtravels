using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KmTravels.Infrastructure.Data.Migrations;

/// <inheritdoc />
public partial class AddServiceOfferings : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "ServiceOfferings",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                Subtitle = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                Slug = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CoverImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                MetaTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                MetaDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                MetaKeywords = table.Column<string>(type: "nvarchar(max)", nullable: true),
                SortOrder = table.Column<int>(type: "int", nullable: false),
                DemandRating = table.Column<int>(type: "int", nullable: false),
                IsPublished = table.Column<bool>(type: "bit", nullable: false),
                IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                ViewCount = table.Column<int>(type: "int", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                IsActive = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ServiceOfferings", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ServiceImages",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ServiceId = table.Column<int>(type: "int", nullable: false),
                Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Caption = table.Column<string>(type: "nvarchar(max)", nullable: true),
                SortOrder = table.Column<int>(type: "int", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                IsActive = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ServiceImages", x => x.Id);
                table.ForeignKey(
                    name: "FK_ServiceImages_ServiceOfferings_ServiceId",
                    column: x => x.ServiceId,
                    principalTable: "ServiceOfferings",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_ServiceImages_ServiceId",
            table: "ServiceImages",
            column: "ServiceId");

        migrationBuilder.CreateIndex(
            name: "IX_ServiceOfferings_Slug",
            table: "ServiceOfferings",
            column: "Slug",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "ServiceImages");
        migrationBuilder.DropTable(name: "ServiceOfferings");
    }
}
