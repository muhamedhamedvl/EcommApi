using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiEcomm.InfraStructure.Migrations
{
    /// <inheritdoc />
    public partial class editedfordtos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Photos");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "Photos",
                newName: "ImageName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageName",
                table: "Photos",
                newName: "Url");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Photos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
