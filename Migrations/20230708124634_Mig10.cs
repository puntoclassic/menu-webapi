using Microsoft.EntityFrameworkCore.Migrations;
#nullable disable
namespace MenuBackend.Migrations
{
    /// <inheritdoc />
    public partial class Mig10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "ShippingCosts",
                table: "Settings",
                type: "REAL",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "TEXT",
                oldNullable: true);
            migrationBuilder.AlterColumn<float>(
                name: "Price",
                table: "Foods",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");
        }
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "ShippingCosts",
                table: "Settings",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "REAL",
                oldNullable: true);
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Foods",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "REAL");
        }
    }
}
