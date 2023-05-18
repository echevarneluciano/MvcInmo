using Microsoft.EntityFrameworkCore;

namespace MvcInmo.Models;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }
    public DbSet<Inmueble> Inmuebles { get; set; }
    public DbSet<Propietario> Propietarios { get; set; }
    public DbSet<Inquilino> Inquilinos { get; set; }
    public DbSet<ContratoApi> ContratosApis { get; set; }
    public DbSet<PagoApi> PagosApis { get; set; }

    public DbSet<Usuario> Usuarios { get; set; }

}