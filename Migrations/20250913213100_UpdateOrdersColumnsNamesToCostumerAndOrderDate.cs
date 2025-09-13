using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrdersColumnsNamesToCostumerAndOrderDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "Orders",
                newName: "OrderDate");

            migrationBuilder.RenameColumn(
                name: "Client",
                table: "Orders",
                newName: "Costumer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderDate",
                table: "Orders",
                newName: "CreationDate");

            migrationBuilder.RenameColumn(
                name: "Costumer",
                table: "Orders",
                newName: "Client");
        }
    }
}
