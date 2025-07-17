using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DreamDay.Migrations
{
    /// <inheritdoc />
    public partial class AddVendorAssignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VendorAssignments",
                columns: table => new
                {
                    VendorAssignmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VendorId = table.Column<int>(type: "int", nullable: false),
                    WeddingId = table.Column<int>(type: "int", nullable: false),
                    AssignedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorAssignments", x => x.VendorAssignmentId);
                    table.ForeignKey(
                        name: "FK_VendorAssignments_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendors",
                        principalColumn: "VendorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VendorAssignments_Weddings_WeddingId",
                        column: x => x.WeddingId,
                        principalTable: "Weddings",
                        principalColumn: "WeddingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VendorAssignments_VendorId",
                table: "VendorAssignments",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorAssignments_WeddingId",
                table: "VendorAssignments",
                column: "WeddingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VendorAssignments");
        }
    }
}
