namespace Hotel.Web.Models;

public partial class TipoHabitacion
{
    public int IdTipoHabitacion { get; set; }
    public string Nombre { get; set; } = null!;
    public decimal PrecioNoche { get; set; }
    public virtual ICollection<Habitacion> Habitaciones { get; set; } = new List<Habitacion>();
}
