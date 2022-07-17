using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WannaEat.Web.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CookingAppliance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CookingAppliance", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Foods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: false),
                    NutritionalValue_KiloCalories = table.Column<double>(type: "double precision", nullable: true),
                    NutritionalValue_Protein = table.Column<double>(type: "double precision", nullable: true),
                    NutritionalValue_Fat = table.Column<double>(type: "double precision", nullable: true),
                    NutritionalValue_Carbohydrates = table.Column<double>(type: "double precision", nullable: true),
                    NutritionalValue_Water = table.Column<double>(type: "double precision", nullable: true),
                    NutritionalValue_Cellulose = table.Column<double>(type: "double precision", nullable: true),
                    NutritionalValue_OrganicAcids = table.Column<double>(type: "double precision", nullable: true),
                    NutritionalValue_GlycemicIndex = table.Column<double>(type: "double precision", nullable: true),
                    NutritionalValue_Cholesterol = table.Column<double>(type: "double precision", nullable: true),
                    NutritionalValue_SaturatedFats = table.Column<double>(type: "double precision", nullable: true),
                    Minerals_Potassium = table.Column<double>(type: "double precision", nullable: true),
                    Minerals_Calcium = table.Column<double>(type: "double precision", nullable: true),
                    Minerals_Magnesium = table.Column<double>(type: "double precision", nullable: true),
                    Minerals_Phosphorus = table.Column<double>(type: "double precision", nullable: true),
                    Minerals_Sodium = table.Column<double>(type: "double precision", nullable: true),
                    Minerals_Zinc = table.Column<double>(type: "double precision", nullable: true),
                    Minerals_Iron = table.Column<double>(type: "double precision", nullable: true),
                    Minerals_Selenium = table.Column<double>(type: "double precision", nullable: true),
                    Minerals_Copper = table.Column<double>(type: "double precision", nullable: true),
                    Minerals_Manganese = table.Column<double>(type: "double precision", nullable: true),
                    Minerals_Fluorine = table.Column<double>(type: "double precision", nullable: true),
                    Minerals_Iodine = table.Column<double>(type: "double precision", nullable: true),
                    Minerals_Sulfur = table.Column<double>(type: "double precision", nullable: true),
                    Minerals_Chromium = table.Column<double>(type: "double precision", nullable: true),
                    Minerals_Silicon = table.Column<double>(type: "double precision", nullable: true),
                    Vitamins_A = table.Column<double>(type: "double precision", nullable: true),
                    Vitamins_B1 = table.Column<double>(type: "double precision", nullable: true),
                    Vitamins_B2 = table.Column<double>(type: "double precision", nullable: true),
                    Vitamins_B3PP = table.Column<double>(type: "double precision", nullable: true),
                    Vitamins_B4 = table.Column<double>(type: "double precision", nullable: true),
                    Vitamins_B5 = table.Column<double>(type: "double precision", nullable: true),
                    Vitamins_B6 = table.Column<double>(type: "double precision", nullable: true),
                    Vitamins_B9 = table.Column<double>(type: "double precision", nullable: true),
                    Vitamins_C = table.Column<double>(type: "double precision", nullable: true),
                    Vitamins_D = table.Column<double>(type: "double precision", nullable: true),
                    Vitamins_E = table.Column<double>(type: "double precision", nullable: true),
                    Vitamins_K = table.Column<double>(type: "double precision", nullable: true),
                    Vitamins_H = table.Column<double>(type: "double precision", nullable: true),
                    Discriminator = table.Column<string>(type: "text", nullable: false),
                    Recipe = table.Column<string>(type: "text", nullable: true),
                    IsFoundational = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Foods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CookingApplianceDish",
                columns: table => new
                {
                    RequiredToCookId = table.Column<int>(type: "integer", nullable: false),
                    UsedInCookingId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CookingApplianceDish", x => new { x.RequiredToCookId, x.UsedInCookingId });
                    table.ForeignKey(
                        name: "FK_CookingApplianceDish_CookingAppliance_RequiredToCookId",
                        column: x => x.RequiredToCookId,
                        principalTable: "CookingAppliance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CookingApplianceDish_Foods_UsedInCookingId",
                        column: x => x.UsedInCookingId,
                        principalTable: "Foods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CookingApplianceDish_UsedInCookingId",
                table: "CookingApplianceDish",
                column: "UsedInCookingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CookingApplianceDish");

            migrationBuilder.DropTable(
                name: "CookingAppliance");

            migrationBuilder.DropTable(
                name: "Foods");
        }
    }
}
