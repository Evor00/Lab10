# Laboratorio S10 Hotel y reservas

Aplicación ASP.NET Core MVC para administrar habitaciones y reservas. Usa EF Core, repositorios asíncronos, filtros por disponibilidad y bajas lógicas de habitaciones. La versión actual usa SQLite local para poder ejecutarse sin una instancia de SQL Server.

## Abrir en VS Code

1. Abre esta carpeta con `code .`.
2. Ejecuta `dotnet restore` y `dotnet run --project Hotel.Web`.

Al iniciar se crea automáticamente `Hotel.Web/hotel.db` con datos de prueba. No necesitas instalar ni configurar SQL Server.

## Comando de scaffold solicitado

Instala una vez: `dotnet tool install --global dotnet-ef --version 10.0.0`

Los scripts en `database/` se conservan como evidencia de la versión SQL Server requerida inicialmente por la guía. Si después necesitas volver a ese proveedor, usa este comando desde `Hotel.Web`:

`dotnet ef dbcontext scaffold "Server=.\\SQLEXPRESS;Database=HotelDB;Trusted_Connection=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer --output-dir Models --context HotelContext --force --no-onconfiguring`

El proyecto ya contiene el modelo final equivalente para poder compilarlo y revisarlo. Si regeneras con tu base, no modifiques los archivos producidos en `Models`.

## Evidencia pendiente al finalizar

Captura: listado/filtro de habitaciones, disponibilidad consultada, creación de reserva, listado y cancelación. Luego inicializa Git, revisa el estado y realiza el commit según indique el docente.
