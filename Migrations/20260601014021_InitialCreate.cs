using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trabajo_Software.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SolicitudesTransporte",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Origen = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Destino = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    FechaServicio = table.Column<DateTime>(type: "TEXT", nullable: false),
                    HoraServicio = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    Observaciones = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    FechaSolicitud = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Estado = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false, defaultValue: "Pendiente")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitudesTransporte", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SolicitudesTransporte");
        }
    }
}
