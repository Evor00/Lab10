namespace Hotel.Web.Models;

public class DisponibilidadHabitacionViewModel
{
    public int IdHabitacion { get; init; }
    public string Numero { get; init; } = null!;
    public string Tipo { get; init; } = null!;
    public decimal PrecioNoche { get; init; }
    public int Noches { get; init; }
    public decimal TotalEstimado { get; init; }
}
