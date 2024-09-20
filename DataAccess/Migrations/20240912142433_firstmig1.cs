using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WA.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class firstmig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountDetails",
                table: "UserPurchaseTransactions");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "UserPurchaseTransactions");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPrice",
                table: "UserPurchaseTransactions",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(7,2)");

            migrationBuilder.AddColumn<string>(
                name: "CVV",
                table: "UserPurchaseTransactions",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CardholderName",
                table: "UserPurchaseTransactions",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreditCardNumber",
                table: "UserPurchaseTransactions",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDate",
                table: "UserPurchaseTransactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CVV",
                table: "UserPurchaseTransactions");

            migrationBuilder.DropColumn(
                name: "CardholderName",
                table: "UserPurchaseTransactions");

            migrationBuilder.DropColumn(
                name: "CreditCardNumber",
                table: "UserPurchaseTransactions");

            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                table: "UserPurchaseTransactions");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPrice",
                table: "UserPurchaseTransactions",
                type: "decimal(7,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AddColumn<string>(
                name: "AccountDetails",
                table: "UserPurchaseTransactions",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "UserPurchaseTransactions",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");
        }
    }
}
