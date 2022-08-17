using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftUniCookbook.Infrastructure.Migrations
{
    public partial class AddedRecipeScore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "Recipes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Score",
                table: "Recipes");
        }
    }
}
