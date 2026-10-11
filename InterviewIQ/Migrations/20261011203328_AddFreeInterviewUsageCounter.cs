using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InterviewIQ.Migrations
{
    /// <inheritdoc />
    public partial class AddFreeInterviewUsageCounter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FreeInterviewsUsed",
                table: "IQ_Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(@"
                UPDATE IQ_Users
                SET FreeInterviewsUsed =
                    CASE WHEN IsFreeTrialUsed = 1 THEN 1 ELSE 0 END;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FreeInterviewsUsed",
                table: "IQ_Users");
        }
    }
}
