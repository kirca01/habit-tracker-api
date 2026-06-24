using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitTracker.API.Migrations
{
    /// <inheritdoc />
    public partial class AddWeeklyGoal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WeeklyGoal",
                table: "Habits",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WeeklyGoal",
                table: "Habits");
        }
    }
}
