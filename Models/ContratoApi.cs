using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcInmo.Models;
[Table("Contratos")]
public class ContratoApi
{
    [Display(Name = "Fecha Inicio")]
    public DateTime? FechaInicio { get; set; }
    [Display(Name = "Código Contrato")]
    public int Id { get; set; }
    [Display(Name = "Fecha Fin")]
    public DateTime? FechaFin { get; set; }
    public int? Precio { get; set; }
    [Display(Name = "Código Inquilino")]
    public int? InquilinoId { get; set; }

    [Display(Name = "Código Inquilino")]
    public int? InmuebleId { get; set; }

    [ForeignKey(nameof(InquilinoId))]
    public Inquilino? Inquilino { get; set; }
    [ForeignKey(nameof(InmuebleId))]
    public Inmueble? Inmueble { get; set; }
    public ContratoApi()
    {

    }
    public ContratoApi(int Id, DateTime FechaInicio, DateTime FechaFin, int Precio,
    int InquilinoId, int InmuebleId, Inquilino Inquilino, Inmueble Inmueble)
    {
        this.FechaInicio = FechaInicio;
        this.FechaFin = FechaFin;
        this.Precio = Precio;
        this.InquilinoId = InquilinoId;
        this.InmuebleId = InmuebleId;
        this.Inquilino = Inquilino;
        this.Inmueble = Inmueble;
        this.Id = Id;
    }
}
