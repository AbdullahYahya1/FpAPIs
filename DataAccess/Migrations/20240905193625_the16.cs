using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WA.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class the16 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Width",
                table: "Products",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(4,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Weight",
                table: "Products",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(4,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Height",
                table: "Products",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(4,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Width",
                table: "Products",
                type: "decimal(4,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Weight",
                table: "Products",
                type: "decimal(4,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Height",
                table: "Products",
                type: "decimal(4,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");
        }
    }
}
