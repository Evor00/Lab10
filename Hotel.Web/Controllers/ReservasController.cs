using Hotel.Web.Models;
using Hotel.Web.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hotel.Web.Controllers;

public class ReservasController : Controller
{
    private readonly ReservaRepositorio _repositorio;
    public ReservasController(ReservaRepositorio repositorio) => _repositorio = repositorio;

    public async Task<IActionResult> Index() => View(await _repositorio.ObtenerTodasAsync());

    public async Task<IActionResult> Disponibles(DateTime? ingreso, DateTime? salida)
    {
        ViewBag.Ingreso = ingreso?.ToString("yyyy-MM-dd");
        ViewBag.Salida = salida?.ToString("yyyy-MM-dd");
        if (!ingreso.HasValue || !salida.HasValue) return View(new List<DisponibilidadHabitacionViewModel>());
        if (salida.Value.Date <= ingreso.Value.Date)
        {
            ModelState.AddModelError(string.Empty, "La fecha de salida debe ser posterior a la fecha de ingreso.");
            return View(new List<DisponibilidadHabitacionViewModel>());
        }
        return View(await _repositorio.ObtenerDisponiblesAsync(ingreso.Value, salida.Value));
    }

    public async Task<IActionResult> Create()
    {
        await CargarListasAsync();
        return View(new Reserva { FechaIngreso = DateTime.Today, FechaSalida = DateTime.Today.AddDays(1) });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Reserva reserva)
    {
        ModelState.Remove(nameof(Reserva.Huesped));
        ModelState.Remove(nameof(Reserva.Habitacion));
        if (reserva.IdHuesped <= 0)
            ModelState.AddModelError(nameof(Reserva.IdHuesped), "Selecciona un huésped.");
        if (reserva.IdHabitacion <= 0)
            ModelState.AddModelError(nameof(Reserva.IdHabitacion), "Selecciona una habitación.");
        if (reserva.FechaSalida.Date <= reserva.FechaIngreso.Date)
            ModelState.AddModelError(nameof(Reserva.FechaSalida), "La fecha de salida debe ser posterior a la fecha de ingreso.");
        if (reserva.FechaIngreso.Date < DateTime.Today)
            ModelState.AddModelError(nameof(Reserva.FechaIngreso), "La fecha de ingreso no puede ser anterior a hoy.");
        if (reserva.FechaSalida.Date > reserva.FechaIngreso.Date &&
            await _repositorio.EstaReservadaAsync(reserva.IdHabitacion, reserva.FechaIngreso, reserva.FechaSalida))
            ModelState.AddModelError(nameof(Reserva.IdHabitacion), "La habitación ya está reservada para ese rango.");
        if (!ModelState.IsValid) { await CargarListasAsync(reserva); return View(reserva); }
        reserva.Estado = "Pendiente";
        await _repositorio.CrearAsync(reserva);
        TempData["Exito"] = "Reserva registrada como pendiente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancelar(int id)
    {
        if (!await _repositorio.CancelarAsync(id)) TempData["Error"] = "Solo se pueden cancelar reservas pendientes o confirmadas.";
        else TempData["Exito"] = "Reserva cancelada correctamente.";
        return RedirectToAction(nameof(Index));
    }

    private async Task CargarListasAsync(Reserva? reserva = null)
    {
        ViewData["Huespedes"] = new SelectList(await _repositorio.ObtenerHuespedesActivosAsync(), "IdHuesped", "NombreCompleto", reserva?.IdHuesped);
        ViewData["Habitaciones"] = new SelectList(await _repositorio.ObtenerHabitacionesActivasAsync(), "IdHabitacion", "Numero", reserva?.IdHabitacion);
    }
}
