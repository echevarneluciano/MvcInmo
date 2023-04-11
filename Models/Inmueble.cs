using System.ComponentModel.DataAnnotations.Schema;

namespace MvcInmo.Models;



public class Inmueble
{
    public int Id { get; set; }
    public string? Direccion { get; set; }
    public int? Ambientes { get; set; }
    public int? Superficie { get; set; }
    public decimal? Latitud { get; set; }
    public decimal? Longitud { get; set; }
    public string? Tipo { get; set; }
    public int PropietarioId { get; set; }
    public int? Estado { get; set; }
    public Double Precio { get; set; }

    [ForeignKey(nameof(PropietarioId))]
    public Propietario? Duenio { get; set; }
    public override string ToString()
    {
        return $"Id: {Id}, Direccion: {Direccion}";
    }
    public Inmueble() { }
}
