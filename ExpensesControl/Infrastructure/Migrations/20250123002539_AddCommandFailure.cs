using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpensesControl.Infrastructure.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class AddCommandFailure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "command_failure",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    command_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    error_details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    request_content = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    trace_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_command_failure", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "idx_command_name",
                table: "command_failure",
                column: "command_name");

            migrationBuilder.CreateIndex(
                name: "idx_trace_id",
                table: "command_failure",
                column: "trace_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "command_failure");
        }
    }
}
