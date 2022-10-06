using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BetsyBryant_Assignment3.Data.Migrations
{
    public partial class readdedMovie : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Poster",
                table: "Movie",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ActorPhoto",
                table: "Actor",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Poster",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "ActorPhoto",
                table: "Actor");
        }
    }
}
