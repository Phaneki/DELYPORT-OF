using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trabajo_Software.Migrations
{
    /// <inheritdoc />
    public partial class AddConductorId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConductorId",
                table: "SolicitudesTransporte",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Conductores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NombreCompleto = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Licencia = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Estado = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conductores", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudesTransporte_ConductorId",
                table: "SolicitudesTransporte",
                column: "ConductorId");

            migrationBuilder.AddForeignKey(
                name: "FK_SolicitudesTransporte_Conductores_ConductorId",
                table: "SolicitudesTransporte",
                column: "ConductorId",
                principalTable: "Conductores",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SolicitudesTransporte_Conductores_ConductorId",
                table: "SolicitudesTransporte");

            migrationBuilder.DropTable(
                name: "Conductores");

            migrationBuilder.DropIndex(
                name: "IX_SolicitudesTransporte_ConductorId",
                table: "SolicitudesTransporte");

            migrationBuilder.DropColumn(
                name: "ConductorId",
                table: "SolicitudesTransporte");
        }
    }
}
