using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace journalApi.Migrations
{
    /// <inheritdoc />
    public partial class storedProcedured : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[DeleteResearchersWithoutJournals]
                AS
                BEGIN
                DELETE FROM Researcher
                WHERE ResearcherId NOT IN (SELECT DISTINCT ResearcherId FROM Journal)
                END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE [dbo].[DeleteResearchersWithoutJournals]");
        }
    }
}
