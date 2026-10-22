using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingMVC.Datos.Migrations
{
    /// <inheritdoc />
    public partial class AddImageUrlAShoe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Shoes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Shoes");
        }
    }
}
