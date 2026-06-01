using Microsoft.EntityFrameworkCore;
using Trabajo_Software.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configurar Entity Framework Core con SQLite
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Data Source=AppTransporte.db";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

var app = builder.Build();

// Crear la base de datos automáticamente si no existe y poblar con datos de prueba
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    // Aplicar migraciones si las hay o crear DB
    dbContext.Database.Migrate();

    // Sembrar conductores si la tabla está vacía
    if (!dbContext.Conductores.Any())
    {
        dbContext.Conductores.AddRange(
            new Trabajo_Software.Models.Conductor { NombreCompleto = "Juan Pérez", Licencia = "LIC-001", Estado = "Disponible" },
            new Trabajo_Software.Models.Conductor { NombreCompleto = "María López", Licencia = "LIC-002", Estado = "Disponible" },
            new Trabajo_Software.Models.Conductor { NombreCompleto = "Carlos Ramírez", Licencia = "LIC-003", Estado = "Ocupado" },
            new Trabajo_Software.Models.Conductor { NombreCompleto = "Ana Gómez", Licencia = "LIC-004", Estado = "Disponible" },
            new Trabajo_Software.Models.Conductor { NombreCompleto = "Luis Torres", Licencia = "LIC-005", Estado = "Inactivo" }
        );
        dbContext.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
