using Hotel.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Web.Repositorios;

public class HabitacionRepositorio
{
    private readonly HotelContext _context;
    public HabitacionRepositorio(HotelContext context) => _context = context;

    public Task<List<TipoHabitacion>> ObtenerTiposAsync() =>
        _context.TipoHabitaciones.AsNoTracking().OrderBy(t => t.Nombre).ToListAsync();

    public Task<List<Habitacion>> ObtenerActivasAsync(int? tipoId) =>
        _context.Habitaciones.AsNoTracking().Include(h => h.TipoHabitacion)
            .Where(h => h.Activo && (!tipoId.HasValue || h.IdTipoHabitacion == tipoId.Value))
            .OrderBy(h => h.Numero).ToListAsync();

    public Task<Habitacion?> ObtenerPorIdAsync(int id) =>
        _context.Habitaciones.AsNoTracking().Include(h => h.TipoHabitacion)
            .FirstOrDefaultAsync(h => h.IdHabitacion == id && h.Activo);

    public Task<bool> NumeroExisteAsync(string numero, int? idExcluir = null) =>
        _context.Habitaciones.AsNoTracking().AnyAsync(h => h.Numero == numero &&
            (!idExcluir.HasValue || h.IdHabitacion != idExcluir.Value));

    public async Task AgregarAsync(Habitacion habitacion)
    {
        _context.Habitaciones.Add(habitacion);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ActualizarAsync(Habitacion habitacion)
    {
        var actual = await _context.Habitaciones.FindAsync(habitacion.IdHabitacion);
        if (actual is null || !actual.Activo) return false;
        actual.Numero = habitacion.Numero;
        actual.IdTipoHabitacion = habitacion.IdTipoHabitacion;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> EliminarLogicoAsync(int id)
    {
        var habitacion = await _context.Habitaciones.FindAsync(id);
        if (habitacion is null || !habitacion.Activo) return false;
        habitacion.Activo = false;
        await _context.SaveChangesAsync();
        return true;
    }
}
