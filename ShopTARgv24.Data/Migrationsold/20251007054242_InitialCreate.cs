using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopTARgv24.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RealEstates",
                table: "RealEstates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FileToDatabase",
                table: "FileToDatabase");

            migrationBuilder.RenameTable(
                name: "RealEstates",
                newName: "RealEstate");

            migrationBuilder.RenameTable(
                name: "FileToDatabase",
                newName: "FileToDatabases");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RealEstate",
                table: "RealEstate",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FileToDatabases",
                table: "FileToDatabases",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RealEstate",
                table: "RealEstate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FileToDatabases",
                table: "FileToDatabases");

            migrationBuilder.RenameTable(
                name: "RealEstate",
                newName: "RealEstates");

            migrationBuilder.RenameTable(
                name: "FileToDatabases",
                newName: "FileToDatabase");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RealEstates",
                table: "RealEstates",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FileToDatabase",
                table: "FileToDatabase",
                column: "Id");
        }
    }
}
