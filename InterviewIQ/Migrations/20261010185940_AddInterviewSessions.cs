using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InterviewIQ.Migrations
{
    /// <inheritdoc />
    public partial class AddInterviewSessions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IQ_InterviewSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DomainId = table.Column<int>(type: "int", nullable: false),
                    DifficultyId = table.Column<int>(type: "int", nullable: false),
                    DurationId = table.Column<int>(type: "int", nullable: false),
                    PersonaId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    IsActive = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IQ_InterviewSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IQ_InterviewSessions_IQ_InterviewDifficulties_DifficultyId",
                        column: x => x.DifficultyId,
                        principalTable: "IQ_InterviewDifficulties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IQ_InterviewSessions_IQ_InterviewDomains_DomainId",
                        column: x => x.DomainId,
                        principalTable: "IQ_InterviewDomains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IQ_InterviewSessions_IQ_InterviewDurations_DurationId",
                        column: x => x.DurationId,
                        principalTable: "IQ_InterviewDurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IQ_InterviewSessions_IQ_InterviewerPersonas_PersonaId",
                        column: x => x.PersonaId,
                        principalTable: "IQ_InterviewerPersonas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IQ_InterviewSessions_IQ_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "IQ_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IQ_InterviewSessionTopics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InterviewSessionId = table.Column<int>(type: "int", nullable: false),
                    TopicId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IQ_InterviewSessionTopics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IQ_InterviewSessionTopics_IQ_InterviewSessions_InterviewSessionId",
                        column: x => x.InterviewSessionId,
                        principalTable: "IQ_InterviewSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IQ_InterviewSessionTopics_IQ_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "IQ_Topics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IQ_InterviewSessions_DifficultyId",
                table: "IQ_InterviewSessions",
                column: "DifficultyId");

            migrationBuilder.CreateIndex(
                name: "IX_IQ_InterviewSessions_DomainId",
                table: "IQ_InterviewSessions",
                column: "DomainId");

            migrationBuilder.CreateIndex(
                name: "IX_IQ_InterviewSessions_DurationId",
                table: "IQ_InterviewSessions",
                column: "DurationId");

            migrationBuilder.CreateIndex(
                name: "IX_IQ_InterviewSessions_PersonaId",
                table: "IQ_InterviewSessions",
                column: "PersonaId");

            migrationBuilder.CreateIndex(
                name: "IX_IQ_InterviewSessions_UserId_CreatedDate",
                table: "IQ_InterviewSessions",
                columns: new[] { "UserId", "CreatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_IQ_InterviewSessionTopics_InterviewSessionId_TopicId",
                table: "IQ_InterviewSessionTopics",
                columns: new[] { "InterviewSessionId", "TopicId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IQ_InterviewSessionTopics_TopicId",
                table: "IQ_InterviewSessionTopics",
                column: "TopicId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IQ_InterviewSessionTopics");

            migrationBuilder.DropTable(
                name: "IQ_InterviewSessions");
        }
    }
}
