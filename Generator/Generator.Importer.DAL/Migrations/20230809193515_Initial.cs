using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Generator.Importer.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileString",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LatinSymbols = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RussianSymbols = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RandomInt = table.Column<int>(type: "int", nullable: false),
                    RandomDouble = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileString", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileString");
        }
    }
}
