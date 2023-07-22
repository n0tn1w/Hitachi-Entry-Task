using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HitachiBE.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DaysDB",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Day = table.Column<int>(type: "int", nullable: false),
                    Temperature = table.Column<int>(type: "int", nullable: false),
                    Wind = table.Column<int>(type: "int", nullable: false),
                    Humidity = table.Column<int>(type: "int", nullable: false),
                    Precipitation = table.Column<int>(type: "int", nullable: false),
                    Lightning = table.Column<bool>(type: "bit", nullable: false),
                    Clouds = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DaysDB", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DaysDB");
        }
    }
}
