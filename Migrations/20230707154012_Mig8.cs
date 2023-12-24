using Microsoft.EntityFrameworkCore.Migrations;
#nullable disable
namespace MenuWebapi.Migrations
{
    /// <inheritdoc />
    public partial class Mig8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Settings",
                newName: "SiteSubtitle");
            migrationBuilder.RenameColumn(
                name: "Key",
                table: "Settings",
                newName: "ShippingCosts");
            migrationBuilder.AddColumn<int>(
                name: "OrderCreatedStateId",
                table: "Settings",
                type: "INTEGER",
                nullable: true);
            migrationBuilder.AddColumn<int>(
                name: "OrderPaidStateId",
                table: "Settings",
                type: "INTEGER",
                nullable: true);
            migrationBuilder.AddColumn<string>(
                name: "SiteName",
                table: "Settings",
                type: "TEXT",
                nullable: true);
            migrationBuilder.CreateIndex(
                name: "IX_Settings_OrderCreatedStateId",
                table: "Settings",
                column: "OrderCreatedStateId");
            migrationBuilder.CreateIndex(
                name: "IX_Settings_OrderPaidStateId",
                table: "Settings",
                column: "OrderPaidStateId");
            migrationBuilder.AddForeignKey(
                name: "FK_Settings_OrderStates_OrderCreatedStateId",
                table: "Settings",
                column: "OrderCreatedStateId",
                principalTable: "OrderStates",
                principalColumn: "Id");
            migrationBuilder.AddForeignKey(
                name: "FK_Settings_OrderStates_OrderPaidStateId",
                table: "Settings",
                column: "OrderPaidStateId",
                principalTable: "OrderStates",
                principalColumn: "Id");
        }
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Settings_OrderStates_OrderCreatedStateId",
                table: "Settings");
            migrationBuilder.DropForeignKey(
                name: "FK_Settings_OrderStates_OrderPaidStateId",
                table: "Settings");
            migrationBuilder.DropIndex(
                name: "IX_Settings_OrderCreatedStateId",
                table: "Settings");
            migrationBuilder.DropIndex(
                name: "IX_Settings_OrderPaidStateId",
                table: "Settings");
            migrationBuilder.DropColumn(
                name: "OrderCreatedStateId",
                table: "Settings");
            migrationBuilder.DropColumn(
                name: "OrderPaidStateId",
                table: "Settings");
            migrationBuilder.DropColumn(
                name: "SiteName",
                table: "Settings");
            migrationBuilder.RenameColumn(
                name: "SiteSubtitle",
                table: "Settings",
                newName: "Value");
            migrationBuilder.RenameColumn(
                name: "ShippingCosts",
                table: "Settings",
                newName: "Key");
        }
    }
}
