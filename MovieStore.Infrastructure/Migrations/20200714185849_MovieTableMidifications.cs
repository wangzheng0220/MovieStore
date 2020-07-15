using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieStore.Infrastructure.Migrations
{
    public partial class MovieTableMidifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BackdropUrl",
                table: "Movie",
                maxLength: 2084,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Movie",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Movie",
                nullable: true,
                defaultValueSql: "getdate()");

            migrationBuilder.AddColumn<string>(
                name: "ImdbUrl",
                table: "Movie",
                maxLength: 2084,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginalLanguage",
                table: "Movie",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PosterUrl",
                table: "Movie",
                maxLength: 2084,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Movie",
                type: "decimal(5, 2)",
                nullable: true,
                defaultValue: 9.9m);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReleaseDate",
                table: "Movie",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RunTime",
                table: "Movie",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tagline",
                table: "Movie",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TmdbUrl",
                table: "Movie",
                maxLength: 2084,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Movie",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Movie",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackdropUrl",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "ImdbUrl",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "OriginalLanguage",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "PosterUrl",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "ReleaseDate",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "RunTime",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "Tagline",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "TmdbUrl",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Movie");
        }
    }
}
