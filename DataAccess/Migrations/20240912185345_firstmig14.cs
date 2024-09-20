using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WA.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class firstmig14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "UserPurchaseTransactions",
                newName: "TransactionStatus");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TransactionStatus",
                table: "UserPurchaseTransactions",
                newName: "Status");
        }
    }
}
