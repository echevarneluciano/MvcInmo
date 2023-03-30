namespace MvcInmo.Models;

public class Contrato
{
    public DateTime? FechaInicio { get; set; }
    public int Id { get; set; }
    public DateTime FechaFin { get; set; }
    public decimal? Precio { get; set; }
    public int? InquilinoId { get; set; }
    public int? InmuebleId { get; set; }
    public Inquilino inquilino { get; set; }
    public Inmueble inmueble { get; set; }
    public Contrato()
    {

    }
}
