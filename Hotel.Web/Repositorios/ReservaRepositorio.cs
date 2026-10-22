using Hotel.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Web.Repositorios;

public class ReservaRepositorio
{
    private readonly HotelContext _context;
    public ReservaRepositorio(HotelContext context) => _context = context;

    public Task<List<Huesped>> ObtenerHuespedesActivosAsync() =>
        _context.Huespedes.AsNoTracking().Where(h => h.Activo).OrderBy(h => h.NombreCompleto).ToListAsync();

    public Task<List<Habitacion>> ObtenerHabitacionesActivasAsync() =>
        _context.Habitaciones.AsNoTracking().Include(h => h.TipoHabitacion).Where(h => h.Activo)
            .OrderBy(h => h.Numero).ToListAsync();

    public async Task<List<DisponibilidadHabitacionViewModel>> ObtenerDisponiblesAsync(DateTime ingreso, DateTime salida)
    {
        var noches = (salida.Date - ingreso.Date).Days;
        var habitaciones = await _context.Habitaciones.AsNoTracking().Include(h => h.TipoHabitacion)
            .Where(h => h.Activo && !h.Reservas.Any(r => r.Estado != "Cancelada" &&
                r.FechaIngreso < salida.Date && r.FechaSalida > ingreso.Date))
            .OrderBy(h => h.Numero).ToListAsync();
        return habitaciones.Select(h => new DisponibilidadHabitacionViewModel
            {
                IdHabitacion = h.IdHabitacion,
                Numero = h.Numero,
                Tipo = h.TipoHabitacion.Nombre,
                PrecioNoche = h.TipoHabitacion.PrecioNoche,
                Noches = noches,
                TotalEstimado = noches * h.TipoHabitacion.PrecioNoche
            }).ToList();
    }

    public Task<bool> EstaReservadaAsync(int idHabitacion, DateTime ingreso, DateTime salida) =>
        _context.Reservas.AsNoTracking().AnyAsync(r => r.IdHabitacion == idHabitacion && r.Estado != "Cancelada" &&
            r.FechaIngreso < salida.Date && r.FechaSalida > ingreso.Date);

    public async Task CrearAsync(Reserva reserva)
    {
        _context.Reservas.Add(reserva);
        await _context.SaveChangesAsync();
    }

    public async Task<List<ReservaListadoViewModel>> ObtenerTodasAsync()
    {
        var reservas = await _context.Reservas.AsNoTracking().Include(r => r.Huesped)
            .Include(r => r.Habitacion).ThenInclude(h => h.TipoHabitacion)
            .OrderByDescending(r => r.FechaIngreso).ToListAsync();
        return reservas.Select(r => new ReservaListadoViewModel
            {
                IdReserva = r.IdReserva,
                Huesped = r.Huesped.NombreCompleto,
                Habitacion = r.Habitacion.Numero + " - " + r.Habitacion.TipoHabitacion.Nombre,
                FechaIngreso = r.FechaIngreso,
                FechaSalida = r.FechaSalida,
                Estado = r.Estado,
                Total = (r.FechaSalida.Date - r.FechaIngreso.Date).Days * r.Habitacion.TipoHabitacion.PrecioNoche
            }).ToList();
    }

    public async Task<bool> CancelarAsync(int id)
    {
        var reserva = await _context.Reservas.FindAsync(id);
        if (reserva is null || reserva.Estado is not ("Pendiente" or "Confirmada")) return false;
        reserva.Estado = "Cancelada";
        await _context.SaveChangesAsync();
        return true;
    }
}
