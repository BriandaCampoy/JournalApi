using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace journalApi.Migrations
{
    /// <inheritdoc />
    public partial class tableUniversity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "University",
                columns: table => new
                {
                    UniversityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    nameUniversity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    city = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_University", x => x.UniversityId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "University");
        }
    }
}
