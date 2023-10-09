using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelectU.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class UserRating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserRatings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicantId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ReviewerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ScholarshipApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Rating = table.Column<int>(type: "int", nullable: true),
                    ScholarshipId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRatings_ScholarshipApplications_ScholarshipApplicationId",
                        column: x => x.ScholarshipApplicationId,
                        principalTable: "ScholarshipApplications",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserRatings_Scholarships_ScholarshipId",
                        column: x => x.ScholarshipId,
                        principalTable: "Scholarships",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserRatings_Users_ApplicantId",
                        column: x => x.ApplicantId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserRatings_Users_ReviewerId",
                        column: x => x.ReviewerId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserRatingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Editted = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_UserRatings_UserRatingId",
                        column: x => x.UserRatingId,
                        principalTable: "UserRatings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserRatingId",
                table: "Comments",
                column: "UserRatingId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRatings_ApplicantId",
                table: "UserRatings",
                column: "ApplicantId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRatings_ReviewerId",
                table: "UserRatings",
                column: "ReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRatings_ScholarshipApplicationId",
                table: "UserRatings",
                column: "ScholarshipApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRatings_ScholarshipId",
                table: "UserRatings",
                column: "ScholarshipId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "UserRatings");
        }
    }
}
