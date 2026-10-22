using Microsoft.EntityFrameworkCore;

namespace Hotel.Web.Models;

public static class DbInitializer
{
    public static async Task InicializarAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HotelContext>();
        await context.Database.EnsureCreatedAsync();

        if (await context.TipoHabitaciones.AnyAsync()) return;

        var tipos = new[]
        {
            new TipoHabitacion { Nombre = "Estándar", PrecioNoche = 150m },
            new TipoHabitacion { Nombre = "Doble", PrecioNoche = 220m },
            new TipoHabitacion { Nombre = "Suite", PrecioNoche = 380m }
        };
        context.TipoHabitaciones.AddRange(tipos);
        await context.SaveChangesAsync();

        var habitaciones = new[]
        {
            new Habitacion { Numero = "101", IdTipoHabitacion = tipos[0].IdTipoHabitacion, Activo = true },
            new Habitacion { Numero = "102", IdTipoHabitacion = tipos[0].IdTipoHabitacion, Activo = true },
            new Habitacion { Numero = "201", IdTipoHabitacion = tipos[1].IdTipoHabitacion, Activo = true },
            new Habitacion { Numero = "202", IdTipoHabitacion = tipos[1].IdTipoHabitacion, Activo = true },
            new Habitacion { Numero = "301", IdTipoHabitacion = tipos[2].IdTipoHabitacion, Activo = true }
        };
        var huespedes = new[]
        {
            new Huesped { NombreCompleto = "Ana Torres", Documento = "70000001", Email = "ana@example.com", Activo = true },
            new Huesped { NombreCompleto = "Luis Díaz", Documento = "70000002", Email = "luis@example.com", Activo = true }
        };
        context.Habitaciones.AddRange(habitaciones);
        context.Huespedes.AddRange(huespedes);
        await context.SaveChangesAsync();

        context.Reservas.Add(new Reserva
        {
            IdHuesped = huespedes[0].IdHuesped,
            IdHabitacion = habitaciones[0].IdHabitacion,
            FechaIngreso = DateTime.Today.AddDays(7),
            FechaSalida = DateTime.Today.AddDays(9),
            Estado = "Confirmada",
            Observaciones = "Reserva de ejemplo"
        });
        await context.SaveChangesAsync();
    }
}
