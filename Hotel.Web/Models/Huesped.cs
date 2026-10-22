namespace Hotel.Web.Models;

public partial class Huesped
{
    public int IdHuesped { get; set; }
    public string NombreCompleto { get; set; } = null!;
    public string Documento { get; set; } = null!;
    public string? Email { get; set; }
    public bool Activo { get; set; }
    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
