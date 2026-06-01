using Microsoft.EntityFrameworkCore;
using Trabajo_Software.Models;

namespace Trabajo_Software.Data;

/// <summary>
/// DbContext principal de la aplicación
/// Configura la conexión a SQLite y las entidades
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Tabla de Solicitudes de Transporte
    /// </summary>
    public DbSet<SolicitudTransporte> SolicitudesTransporte { get; set; }

    /// <summary>
    /// Tabla de Conductores
    /// </summary>
    public DbSet<Conductor> Conductores { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración de la entidad SolicitudTransporte
        modelBuilder.Entity<SolicitudTransporte>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Origen)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Destino)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.FechaServicio)
                .IsRequired();

            entity.Property(e => e.HoraServicio)
                .IsRequired();

            entity.Property(e => e.Observaciones)
                .HasMaxLength(500);

            entity.Property(e => e.FechaSolicitud)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.Property(e => e.Estado)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("Pendiente");

            entity.Property(e => e.IdentificadorUnico)
                .IsRequired()
                .HasMaxLength(50);
                
            entity.HasIndex(e => e.IdentificadorUnico)
                .IsUnique();

            entity.Property(e => e.ObservacionesValidacion)
                .HasMaxLength(500);

            entity.HasOne(e => e.Conductor)
                .WithMany()
                .HasForeignKey(e => e.ConductorId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
