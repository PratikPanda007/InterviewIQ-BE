using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InterviewIQ.Migrations
{
    /// <inheritdoc />
    public partial class AddInterviewBillingFoundation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IQ_Subscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PlanName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CurrencyCode = table.Column<string>(type: "varchar(3)", unicode: false, maxLength: 3, nullable: false),
                    PriceAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    InterviewCreditsGranted = table.Column<int>(type: "int", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CurrentPeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CurrentPeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ProviderSubscriptionId = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    IsActive = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IQ_Subscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IQ_Subscriptions_IQ_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "IQ_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IQ_UserMonthlyUsage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    FreeInterviewsUsed = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IQ_UserMonthlyUsage", x => x.Id);
                    table.CheckConstraint("CK_IQ_UserMonthlyUsage_FreeInterviewsUsed", "[FreeInterviewsUsed] >= 0");
                    table.CheckConstraint("CK_IQ_UserMonthlyUsage_Month", "[Month] BETWEEN 1 AND 12");
                    table.ForeignKey(
                        name: "FK_IQ_UserMonthlyUsage_IQ_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "IQ_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IQ_PaymentTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SubscriptionId = table.Column<int>(type: "int", nullable: true),
                    Provider = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CurrencyCode = table.Column<string>(type: "varchar(3)", unicode: false, maxLength: 3, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ProviderOrderId = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ProviderPaymentId = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    IdempotencyKey = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    IsActive = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IQ_PaymentTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IQ_PaymentTransactions_IQ_Subscriptions_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "IQ_Subscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IQ_PaymentTransactions_IQ_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "IQ_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IQ_UserInterviewCredits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    GrantedCredits = table.Column<int>(type: "int", nullable: false),
                    RemainingCredits = table.Column<int>(type: "int", nullable: false),
                    PaymentTransactionId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IQ_UserInterviewCredits", x => x.Id);
                    table.CheckConstraint("CK_IQ_UserInterviewCredits_Balances", "[GrantedCredits] > 0 AND [RemainingCredits] >= 0 AND [RemainingCredits] <= [GrantedCredits]");
                    table.ForeignKey(
                        name: "FK_IQ_UserInterviewCredits_IQ_PaymentTransactions_PaymentTransactionId",
                        column: x => x.PaymentTransactionId,
                        principalTable: "IQ_PaymentTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IQ_UserInterviewCredits_IQ_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "IQ_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IQ_PaymentTransactions_IdempotencyKey",
                table: "IQ_PaymentTransactions",
                column: "IdempotencyKey",
                unique: true,
                filter: "[IdempotencyKey] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_IQ_PaymentTransactions_ProviderPaymentId",
                table: "IQ_PaymentTransactions",
                column: "ProviderPaymentId",
                unique: true,
                filter: "[ProviderPaymentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_IQ_PaymentTransactions_SubscriptionId",
                table: "IQ_PaymentTransactions",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_IQ_PaymentTransactions_UserId",
                table: "IQ_PaymentTransactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_IQ_Subscriptions_ProviderSubscriptionId",
                table: "IQ_Subscriptions",
                column: "ProviderSubscriptionId",
                unique: true,
                filter: "[ProviderSubscriptionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_IQ_Subscriptions_UserId",
                table: "IQ_Subscriptions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_IQ_UserInterviewCredits_PaymentTransactionId",
                table: "IQ_UserInterviewCredits",
                column: "PaymentTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_IQ_UserInterviewCredits_UserId",
                table: "IQ_UserInterviewCredits",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_IQ_UserMonthlyUsage_UserId_Year_Month",
                table: "IQ_UserMonthlyUsage",
                columns: new[] { "UserId", "Year", "Month" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IQ_UserInterviewCredits");

            migrationBuilder.DropTable(
                name: "IQ_UserMonthlyUsage");

            migrationBuilder.DropTable(
                name: "IQ_PaymentTransactions");

            migrationBuilder.DropTable(
                name: "IQ_Subscriptions");
        }
    }
}
