using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trabajo_Software.Migrations
{
    /// <inheritdoc />
    public partial class AddObservacionesValidacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ObservacionesValidacion",
                table: "SolicitudesTransporte",
                type: "TEXT",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ObservacionesValidacion",
                table: "SolicitudesTransporte");
        }
    }
}
