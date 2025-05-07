using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniversalCarShop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initialmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    Number = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Engine = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.Number);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    LegPower = table.Column<int>(type: "INTEGER", nullable: false),
                    HandPower = table.Column<int>(type: "INTEGER", nullable: false),
                    CarNumber = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Name);
                    table.ForeignKey(
                        name: "FK_Customers_Cars_CarNumber",
                        column: x => x.CarNumber,
                        principalTable: "Cars",
                        principalColumn: "Number");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CarNumber",
                table: "Customers",
                column: "CarNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Cars");
        }
    }
}
