using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DreamDay.Migrations
{
    /// <inheritdoc />
    public partial class AddWeddingTablesandGuestSeating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfPeople",
                table: "Guests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TableId",
                table: "Guests",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WeddingTable",
                columns: table => new
                {
                    TableId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WeddingId = table.Column<int>(type: "int", nullable: false),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxSeats = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeddingTable", x => x.TableId);
                    table.ForeignKey(
                        name: "FK_WeddingTable_Weddings_WeddingId",
                        column: x => x.WeddingId,
                        principalTable: "Weddings",
                        principalColumn: "WeddingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Guests_TableId",
                table: "Guests",
                column: "TableId");

            migrationBuilder.CreateIndex(
                name: "IX_WeddingTable_WeddingId",
                table: "WeddingTable",
                column: "WeddingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Guests_WeddingTable_TableId",
                table: "Guests",
                column: "TableId",
                principalTable: "WeddingTable",
                principalColumn: "TableId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guests_WeddingTable_TableId",
                table: "Guests");

            migrationBuilder.DropTable(
                name: "WeddingTable");

            migrationBuilder.DropIndex(
                name: "IX_Guests_TableId",
                table: "Guests");

            migrationBuilder.DropColumn(
                name: "NumberOfPeople",
                table: "Guests");

            migrationBuilder.DropColumn(
                name: "TableId",
                table: "Guests");
        }
    }
}
