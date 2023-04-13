namespace MvcInmo.Models;

public class Pago
{
    public int Id { get; set; }
    public DateTime? FechaPagado { get; set; }
    public int Mes { get; set; }
    public int ContratoId { get; set; }
    public Decimal? Importe { get; set; }
    public Contrato contrato { get; set; }
    public Pago(DateTime FechaPagado, int Mes, int ContratoId, Contrato contrato)
    {
        this.FechaPagado = FechaPagado;
        this.Mes = Mes;
        this.ContratoId = ContratoId;
        this.contrato = contrato;
    }
    public Pago() { }
    public override string ToString()
    {
        return $"Identificador: {Id}, N°: {Mes}, Contrato Id: {ContratoId}";
    }
}
