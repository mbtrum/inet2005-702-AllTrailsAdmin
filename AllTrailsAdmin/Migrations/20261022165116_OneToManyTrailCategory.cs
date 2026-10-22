using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AllTrailsAdmin.Migrations
{
    /// <inheritdoc />
    public partial class OneToManyTrailCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Trail");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Trail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trail_CategoryId",
                table: "Trail",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trail_Category_CategoryId",
                table: "Trail",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trail_Category_CategoryId",
                table: "Trail");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropIndex(
                name: "IX_Trail_CategoryId",
                table: "Trail");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Trail");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Trail",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
