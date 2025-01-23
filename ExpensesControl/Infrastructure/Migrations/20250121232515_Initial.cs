using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpensesControl.Infrastructure.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "expenses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_code = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: true),
                    category = table.Column<int>(type: "int", nullable: false),
                    is_recurring = table.Column<bool>(type: "bit", nullable: false),
                    recurrence_periodicity = table.Column<int>(type: "int", nullable: false),
                    max_occurrences = table.Column<int>(type: "int", nullable: true),
                    payment_method_type = table.Column<int>(type: "int", nullable: false),
                    Payment_IsInstallment = table.Column<bool>(type: "bit", nullable: false),
                    installment_count = table.Column<int>(type: "int", nullable: true),
                    payment_notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    total_value = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    creation_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    created_by_user = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    last_update_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    updated_by_user = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expenses", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "expenses");
        }
    }
}
