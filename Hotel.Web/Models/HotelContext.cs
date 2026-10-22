using Microsoft.EntityFrameworkCore;

namespace Hotel.Web.Models;

public partial class HotelContext : DbContext
{
    public HotelContext(DbContextOptions<HotelContext> options) : base(options) { }

    public virtual DbSet<TipoHabitacion> TipoHabitaciones { get; set; }
    public virtual DbSet<Habitacion> Habitaciones { get; set; }
    public virtual DbSet<Huesped> Huespedes { get; set; }
    public virtual DbSet<Reserva> Reservas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TipoHabitacion>(entity =>
        {
            entity.HasKey(e => e.IdTipoHabitacion);
            entity.ToTable("TipoHabitacion");
            entity.Property(e => e.Nombre).HasMaxLength(80);
            entity.Property(e => e.PrecioNoche).HasConversion<double>();
        });

        modelBuilder.Entity<Habitacion>(entity =>
        {
            entity.HasKey(e => e.IdHabitacion);
            entity.ToTable("Habitacion");
            entity.HasIndex(e => e.Numero).IsUnique();
            entity.Property(e => e.Numero).HasMaxLength(10);
            entity.HasOne(e => e.TipoHabitacion).WithMany(e => e.Habitaciones)
                .HasForeignKey(e => e.IdTipoHabitacion).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Huesped>(entity =>
        {
            entity.HasKey(e => e.IdHuesped);
            entity.ToTable("Huesped");
            entity.HasIndex(e => e.Documento).IsUnique();
            entity.Property(e => e.NombreCompleto).HasMaxLength(150);
            entity.Property(e => e.Documento).HasMaxLength(20);
            entity.Property(e => e.Email).HasMaxLength(120);
        });

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.HasKey(e => e.IdReserva);
            entity.ToTable("Reserva");
            entity.Property(e => e.Estado).HasMaxLength(15);
            entity.Property(e => e.Observaciones).HasMaxLength(200);
            entity.HasOne(e => e.Huesped).WithMany(e => e.Reservas)
                .HasForeignKey(e => e.IdHuesped).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Habitacion).WithMany(e => e.Reservas)
                .HasForeignKey(e => e.IdHabitacion).OnDelete(DeleteBehavior.Restrict);
        });
    }
}
