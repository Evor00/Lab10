namespace Hotel.Web.Models;

public partial class Reserva
{
    public int IdReserva { get; set; }
    public int IdHuesped { get; set; }
    public int IdHabitacion { get; set; }
    public DateTime FechaIngreso { get; set; }
    public DateTime FechaSalida { get; set; }
    public string Estado { get; set; } = null!;
    public string? Observaciones { get; set; }
    public virtual Huesped Huesped { get; set; } = null!;
    public virtual Habitacion Habitacion { get; set; } = null!;
}
