using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcInmo.Models;

[Table("Pagos")]
public class PagoApi
{
    [Display(Name = "Código Pago")]
    public int Id { get; set; }
    [Display(Name = "Fecha de pago")]
    public DateTime? FechaPagado { get; set; }
    [Display(Name = "N° Cuota")]
    public int Mes { get; set; }
    [Display(Name = "Código Contrato")]
    public int ContratoId { get; set; }
    [ForeignKey(nameof(ContratoId))]
    public Decimal? Importe { get; set; }
    public ContratoApi contrato { get; set; }
    //public Inquilino inquilino { get; set; }
    public PagoApi(DateTime FechaPagado, int Mes, int ContratoId, ContratoApi contrato)
    {
        this.FechaPagado = FechaPagado;
        this.Mes = Mes;
        this.ContratoId = ContratoId;
        this.contrato = contrato;
    }
    public PagoApi() { }
    public override string ToString()
    {
        return $"Identificador: {Id}, N°: {Mes}, Contrato Id: {ContratoId}";
    }
}
