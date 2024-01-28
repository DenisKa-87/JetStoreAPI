using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JetStoreAPI.Data.Migrations
{
    public partial class MeasureUnitEdited : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IntOnly",
                table: "MeasureUnits",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IntOnly",
                table: "MeasureUnits");
        }
    }
}
