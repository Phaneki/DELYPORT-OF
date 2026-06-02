# Sistema de Gestión de Solicitudes de Transporte 🚚

![.NET Core](https://img.shields.io/badge/.NET%20Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)
![SQLite](https://img.shields.io/badge/sqlite-%2307405e.svg?style=for-the-badge&logo=sqlite&logoColor=white)
![Bootstrap](https://img.shields.io/badge/bootstrap-%238511FA.svg?style=for-the-badge&logo=bootstrap&logoColor=white)

Un sistema de software robusto desarrollado en **ASP.NET Core MVC** que digitaliza y automatiza el flujo completo de una empresa de transportes y logística. 

Este proyecto fue planificado y ejecutado bajo el marco de trabajo **Scrum**, garantizando entregas de valor mediante Historias de Usuario (HU) bien definidas, asegurando trazabilidad y cumplimiento de los requerimientos del cliente.

---

## 🌟 Características Principales (Features)

El sistema está dividido funcionalmente en los roles dictados por las Historias de Usuario del **Sprint 1**:

1. **Cliente (HU-001 - Registro)**
   - Formulario avanzado de cotización dinámica basado en distancia y volumen de carga.
   - Restricciones de seguridad, formato de caracteres y prevenciones de fechas vencidas.
   - Generación automática de tickets/identificadores únicos por solicitud.

2. **Operador Logístico (HU-003 - Validación)**
   - Dashboard de monitoreo con estadísticas en tiempo real.
   - Bandeja de entrada de solicitudes pendientes.
   - Capacidad de **Aprobar** o **Rechazar** servicios.
   - Auditoría de validación (Registro de justificaciones y observaciones adjuntas a la solicitud).

3. **Asignación y Conductores (HU-004 - Asignación)**
   - Directorio inteligente de conductores.
   - Gestión de estado en tiempo real (Conductores disponibles vs ocupados).
   - Enlace relacional entre conductor y solicitud (Foreign Keys en base de datos).
   - Portal exclusivo del chofer (`/ConductorUI`) para recibir la ruta, detalles de la carga y cobro estimado.

---

## 🛠️ Stack Tecnológico

* **Backend:** C# con ASP.NET Core MVC (Versión 10.0)
* **Base de Datos:** SQLite local (`AppTransporte.db`)
* **ORM:** Entity Framework Core (Code-First con Migrations)
* **Frontend:** HTML5, CSS3, JavaScript Vainilla
* **UI/UX Framework:** Bootstrap 5 (Iconos, Alertas dinámicas, Modales, Grids)

---

## ⚙️ Instrucciones de Instalación y Uso

Para ejecutar el proyecto en un entorno local, asegúrese de cumplir con los siguientes requisitos previos:
- [SDK de .NET 10](https://dotnet.microsoft.com/download) instalado en su máquina.
- Un IDE compatible (Visual Studio 2022 o Visual Studio Code con la extensión de C#).

### Paso 1: Clonar el Repositorio
```bash
git clone <URL_DEL_REPOSITORIO>
cd "Trabajo Software"
```

### Paso 2: Restaurar Dependencias
Este comando descargará todos los paquetes NuGet requeridos por Entity Framework Core.
```bash
dotnet restore
```

### Paso 3: Aplicar las Migraciones (Base de Datos)
El proyecto utiliza Entity Framework Code-First. La base de datos SQLite se creará automáticamente en la carpeta raíz al aplicar la última migración.
```bash
dotnet ef database update
```
*(Nota: Si no tiene la herramienta global de EF instalada, ejecute `dotnet tool install --global dotnet-ef` primero).*

### Paso 4: Ejecutar el Proyecto
Para iniciar el servidor local con soporte para recarga en caliente (Hot Reload):
```bash
dotnet watch
```
Alternativamente, puede usar la compilación normal:
```bash
dotnet run
```

### Paso 5: Navegación
Una vez que el servidor esté corriendo, el navegador se abrirá automáticamente en `https://localhost:<puerto>`.
- Portal de Solicitudes: `/Solicitud`
- Portal de Validación (Operador): `/Solicitud/PendientesValidacion`
- Directorio de Choferes: `/Asignacion/ConductoresDisponibles`
- Portal del Conductor: `/ConductorUI`

---

## 📈 Metodología y Control de Versiones

Este repositorio refleja una historia de commits meticulosamente diseñada para demostrar el trabajo iterativo de **Scrum**.
- Cada **Historia de Usuario (US)** tiene su propia rama de características (Ej: `US-01-Registro`).
- Cada rama contiene una serie de **Tareas (TASK)** atómicas que corresponden a componentes individuales del software (Back-end, Vistas, Validaciones).
- Todo ha sido integrado progresivamente a la rama principal `main` respetando los criterios de aceptación QA.

Desarrollado con altos estándares de codificación por el Equipo de Desarrollo.
