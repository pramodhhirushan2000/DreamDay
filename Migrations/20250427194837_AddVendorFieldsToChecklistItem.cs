using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DreamDay.Migrations
{
    /// <inheritdoc />
    public partial class AddVendorFieldsToChecklistItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VendorCategory",
                table: "ChecklistItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "VendorId",
                table: "ChecklistItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistItems_VendorId",
                table: "ChecklistItems",
                column: "VendorId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChecklistItems_Vendors_VendorId",
                table: "ChecklistItems",
                column: "VendorId",
                principalTable: "Vendors",
                principalColumn: "VendorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChecklistItems_Vendors_VendorId",
                table: "ChecklistItems");

            migrationBuilder.DropIndex(
                name: "IX_ChecklistItems_VendorId",
                table: "ChecklistItems");

            migrationBuilder.DropColumn(
                name: "VendorCategory",
                table: "ChecklistItems");

            migrationBuilder.DropColumn(
                name: "VendorId",
                table: "ChecklistItems");
        }
    }
}
