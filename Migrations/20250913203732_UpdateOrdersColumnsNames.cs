using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace order_management_system.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrdersColumnsNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Valor",
                table: "Orders",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "Produto",
                table: "Orders",
                newName: "Product");

            migrationBuilder.RenameColumn(
                name: "DataCriacao",
                table: "Orders",
                newName: "CreationDate");

            migrationBuilder.RenameColumn(
                name: "Cliente",
                table: "Orders",
                newName: "Client");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Orders",
                newName: "Valor");

            migrationBuilder.RenameColumn(
                name: "Product",
                table: "Orders",
                newName: "Produto");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "Orders",
                newName: "DataCriacao");

            migrationBuilder.RenameColumn(
                name: "Client",
                table: "Orders",
                newName: "Cliente");
        }
    }
}
