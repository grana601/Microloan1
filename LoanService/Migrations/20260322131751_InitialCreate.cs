using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "loanaction",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    loanid = table.Column<Guid>(type: "uuid", nullable: false),
                    actiontype = table.Column<string>(type: "text", nullable: false),
                    notes = table.Column<string>(type: "text", nullable: false),
                    createdat = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_loanaction", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "loanagreement",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    loanid = table.Column<Guid>(type: "uuid", nullable: false),
                    filepath = table.Column<string>(type: "text", nullable: false),
                    issigned = table.Column<bool>(type: "boolean", nullable: false),
                    signedat = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    createdat = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_loanagreement", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "loaninterest",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    rate = table.Column<decimal>(type: "numeric", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    effectivefrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_loaninterest", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "loans",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    customerid = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    termmonths = table.Column<int>(type: "integer", nullable: false),
                    interestrate = table.Column<decimal>(type: "numeric", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    purpose = table.Column<string>(type: "text", nullable: false),
                    createdat = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updatedat = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_loans", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "penaltyinterest",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    dailyrate = table.Column<decimal>(type: "numeric", nullable: false),
                    graceperioddays = table.Column<int>(type: "integer", nullable: false),
                    effectivefrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_penaltyinterest", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "loanaction");

            migrationBuilder.DropTable(
                name: "loanagreement");

            migrationBuilder.DropTable(
                name: "loaninterest");

            migrationBuilder.DropTable(
                name: "loans");

            migrationBuilder.DropTable(
                name: "penaltyinterest");
        }
    }
}
