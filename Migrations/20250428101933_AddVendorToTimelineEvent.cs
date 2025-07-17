using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DreamDay.Migrations
{
    /// <inheritdoc />
    public partial class AddVendorToTimelineEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "VendorArrivalTime",
                table: "TimelineEvents",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VendorId",
                table: "TimelineEvents",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TimelineEvents_VendorId",
                table: "TimelineEvents",
                column: "VendorId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimelineEvents_Vendors_VendorId",
                table: "TimelineEvents",
                column: "VendorId",
                principalTable: "Vendors",
                principalColumn: "VendorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimelineEvents_Vendors_VendorId",
                table: "TimelineEvents");

            migrationBuilder.DropIndex(
                name: "IX_TimelineEvents_VendorId",
                table: "TimelineEvents");

            migrationBuilder.DropColumn(
                name: "VendorArrivalTime",
                table: "TimelineEvents");

            migrationBuilder.DropColumn(
                name: "VendorId",
                table: "TimelineEvents");
        }
    }
}
