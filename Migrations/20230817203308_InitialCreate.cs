using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace journalApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Researcher",
                columns: table => new
                {
                    ResearcherId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Researcher", x => x.ResearcherId);
                });

            migrationBuilder.CreateTable(
                name: "Journal",
                columns: table => new
                {
                    JournalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResearcherId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PublishedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InternalUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Journal", x => x.JournalId);
                    table.ForeignKey(
                        name: "FK_Journal_Researcher_ResearcherId",
                        column: x => x.ResearcherId,
                        principalTable: "Researcher",
                        principalColumn: "ResearcherId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subscription",
                columns: table => new
                {
                    SubscriptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResearcherId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FollowedResearcherId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscription", x => x.SubscriptionId);
                    table.ForeignKey(
                        name: "FK_Subscription_Researcher_FollowedResearcherId",
                        column: x => x.FollowedResearcherId,
                        principalTable: "Researcher",
                        principalColumn: "ResearcherId");
                    table.ForeignKey(
                        name: "FK_Subscription_Researcher_ResearcherId",
                        column: x => x.ResearcherId,
                        principalTable: "Researcher",
                        principalColumn: "ResearcherId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Journal_ResearcherId",
                table: "Journal",
                column: "ResearcherId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_FollowedResearcherId",
                table: "Subscription",
                column: "FollowedResearcherId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_ResearcherId",
                table: "Subscription",
                column: "ResearcherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Journal");

            migrationBuilder.DropTable(
                name: "Subscription");

            migrationBuilder.DropTable(
                name: "Researcher");
        }
    }
}
