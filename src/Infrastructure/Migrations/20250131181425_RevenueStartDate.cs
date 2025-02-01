using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpensesControl.Infrastructure.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class RevenueStartDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "receipt_date",
                table: "revenues",
                newName: "start_date");

            migrationBuilder.AddColumn<DateOnly>(
                name: "end_date",
                table: "revenues",
                type: "date",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "end_date",
                table: "revenues");

            migrationBuilder.RenameColumn(
                name: "start_date",
                table: "revenues",
                newName: "receipt_date");
        }
    }
}
