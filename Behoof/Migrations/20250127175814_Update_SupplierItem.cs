using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Behoof.Migrations
{
    /// <inheritdoc />
    public partial class Update_SupplierItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MaxPrice",
                table: "SupplierItem",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MinPrice",
                table: "SupplierItem",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxPrice",
                table: "SupplierItem");

            migrationBuilder.DropColumn(
                name: "MinPrice",
                table: "SupplierItem");
        }
    }
}
