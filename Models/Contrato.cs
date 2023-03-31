using System.ComponentModel.DataAnnotations.Schema;

namespace MvcInmo.Models;

public class Contrato
{
    public DateTime? FechaInicio { get; set; }
    public int Id { get; set; }
    public DateTime FechaFin { get; set; }
    public decimal? Precio { get; set; }
    public int InquilinoId { get; set; }
    [ForeignKey(nameof(InquilinoId))]
    public int InmuebleId { get; set; }
    [ForeignKey(nameof(InmuebleId))]
    public Inquilino inquilino { get; set; }
    public Inmueble inmueble { get; set; }
    public Propietario propietario { get; set; }
    public Contrato()
    {

    }
    public Contrato(DateTime FechaInicio, DateTime FechaFin, decimal Precio,
    int InquilinoId, int InmuebleId, Inquilino inquilino, Inmueble inmueble, Propietario propietario)
    {
        this.FechaInicio = FechaInicio;
        this.FechaFin = FechaFin;
        this.Precio = Precio;
        this.InquilinoId = InquilinoId;
        this.InmuebleId = InmuebleId;
        this.inquilino = inquilino;
        this.inmueble = inmueble;
        this.propietario = propietario;
    }
}
