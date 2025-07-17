using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DreamDay.Migrations
{
    /// <inheritdoc />
    public partial class AddWeddingTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guests_WeddingTable_TableId",
                table: "Guests");

            migrationBuilder.DropForeignKey(
                name: "FK_WeddingTable_Weddings_WeddingId",
                table: "WeddingTable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WeddingTable",
                table: "WeddingTable");

            migrationBuilder.RenameTable(
                name: "WeddingTable",
                newName: "WeddingTables");

            migrationBuilder.RenameIndex(
                name: "IX_WeddingTable_WeddingId",
                table: "WeddingTables",
                newName: "IX_WeddingTables_WeddingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WeddingTables",
                table: "WeddingTables",
                column: "TableId");

            migrationBuilder.AddForeignKey(
                name: "FK_Guests_WeddingTables_TableId",
                table: "Guests",
                column: "TableId",
                principalTable: "WeddingTables",
                principalColumn: "TableId");

            migrationBuilder.AddForeignKey(
                name: "FK_WeddingTables_Weddings_WeddingId",
                table: "WeddingTables",
                column: "WeddingId",
                principalTable: "Weddings",
                principalColumn: "WeddingId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guests_WeddingTables_TableId",
                table: "Guests");

            migrationBuilder.DropForeignKey(
                name: "FK_WeddingTables_Weddings_WeddingId",
                table: "WeddingTables");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WeddingTables",
                table: "WeddingTables");

            migrationBuilder.RenameTable(
                name: "WeddingTables",
                newName: "WeddingTable");

            migrationBuilder.RenameIndex(
                name: "IX_WeddingTables_WeddingId",
                table: "WeddingTable",
                newName: "IX_WeddingTable_WeddingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WeddingTable",
                table: "WeddingTable",
                column: "TableId");

            migrationBuilder.AddForeignKey(
                name: "FK_Guests_WeddingTable_TableId",
                table: "Guests",
                column: "TableId",
                principalTable: "WeddingTable",
                principalColumn: "TableId");

            migrationBuilder.AddForeignKey(
                name: "FK_WeddingTable_Weddings_WeddingId",
                table: "WeddingTable",
                column: "WeddingId",
                principalTable: "Weddings",
                principalColumn: "WeddingId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
