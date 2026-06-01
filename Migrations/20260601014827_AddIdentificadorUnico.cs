using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trabajo_Software.Migrations
{
    /// <inheritdoc />
    public partial class AddIdentificadorUnico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdentificadorUnico",
                table: "SolicitudesTransporte",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudesTransporte_IdentificadorUnico",
                table: "SolicitudesTransporte",
                column: "IdentificadorUnico",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SolicitudesTransporte_IdentificadorUnico",
                table: "SolicitudesTransporte");

            migrationBuilder.DropColumn(
                name: "IdentificadorUnico",
                table: "SolicitudesTransporte");
        }
    }
}
