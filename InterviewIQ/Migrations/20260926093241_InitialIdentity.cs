using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InterviewIQ.Migrations
{
    /// <inheritdoc />
    public partial class InitialIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IQ_Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IQ_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IQ_Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsFreeTrialUsed = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IQ_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IQ_Users_IQ_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "IQ_Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IQ_RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IQ_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IQ_RefreshTokens_IQ_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "IQ_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "IQ_Roles",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "IsActive", "Name", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Super Admin", 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Admin", 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "User", 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_IQ_RefreshTokens_Token",
                table: "IQ_RefreshTokens",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IQ_RefreshTokens_UserId",
                table: "IQ_RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_IQ_Roles_Name",
                table: "IQ_Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IQ_Users_Email",
                table: "IQ_Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IQ_Users_RoleId",
                table: "IQ_Users",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IQ_RefreshTokens");

            migrationBuilder.DropTable(
                name: "IQ_Users");

            migrationBuilder.DropTable(
                name: "IQ_Roles");
        }
    }
}
