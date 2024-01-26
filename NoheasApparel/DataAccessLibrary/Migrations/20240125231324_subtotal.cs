using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoheasApparel.DataAccessLibrary.Migrations
{
    /// <inheritdoc />
    public partial class subtotal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "SubTotal",
                table: "OrderHeaders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Tax",
                table: "OrderHeaders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubTotal",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "Tax",
                table: "OrderHeaders");
        }
    }
}
