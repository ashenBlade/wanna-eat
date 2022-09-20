using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

#nullable disable

namespace WannaEat.Web.Migrations
{
    public partial class ReplaceFullTextSearchIndexWithStringByNameIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ingredients_NameSearchVector",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "NameSearchVector",
                table: "Ingredients");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_Name",
                table: "Ingredients",
                column: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ingredients_Name",
                table: "Ingredients");

            migrationBuilder.AddColumn<NpgsqlTsVector>(
                name: "NameSearchVector",
                table: "Ingredients",
                type: "tsvector",
                nullable: false)
                .Annotation("Npgsql:TsVectorConfig", "russian")
                .Annotation("Npgsql:TsVectorProperties", new[] { "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_NameSearchVector",
                table: "Ingredients",
                column: "NameSearchVector")
                .Annotation("Npgsql:IndexMethod", "GIN");
        }
    }
}
