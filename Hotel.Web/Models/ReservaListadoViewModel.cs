namespace Hotel.Web.Models;

public class ReservaListadoViewModel
{
    public int IdReserva { get; init; }
    public string Huesped { get; init; } = null!;
    public string Habitacion { get; init; } = null!;
    public DateTime FechaIngreso { get; init; }
    public DateTime FechaSalida { get; init; }
    public string Estado { get; init; } = null!;
    public decimal Total { get; init; }
    public bool PuedeCancelar => Estado is "Pendiente" or "Confirmada";
}
