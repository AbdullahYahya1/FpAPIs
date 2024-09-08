using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WA.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class the19 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "ServiceRequests",
                newName: "ServiceRequestStatus");

            migrationBuilder.AddColumn<decimal>(
                name: "PurchasePrice",
                table: "ServiceRequests",
                type: "decimal(7,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PurchasePrice",
                table: "ServiceRequests");

            migrationBuilder.RenameColumn(
                name: "ServiceRequestStatus",
                table: "ServiceRequests",
                newName: "Status");
        }
    }
}
