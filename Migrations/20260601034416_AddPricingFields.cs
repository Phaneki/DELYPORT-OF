using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trabajo_Software.Migrations
{
    /// <inheritdoc />
    public partial class AddPricingFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DimensionesProducto",
                table: "SolicitudesTransporte",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DistritoDestino",
                table: "SolicitudesTransporte",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "PrecioEstimado",
                table: "SolicitudesTransporte",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DimensionesProducto",
                table: "SolicitudesTransporte");

            migrationBuilder.DropColumn(
                name: "DistritoDestino",
                table: "SolicitudesTransporte");

            migrationBuilder.DropColumn(
                name: "PrecioEstimado",
                table: "SolicitudesTransporte");
        }
    }
}
