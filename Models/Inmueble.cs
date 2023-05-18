using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcInmo.Models;



public class Inmueble
{
    [Display(Name = "Código Inmueble")]
    public int Id { get; set; }
    public string? Direccion { get; set; }
    public int? Ambientes { get; set; }
    public int? Superficie { get; set; }
    public decimal? Latitud { get; set; }
    public decimal? Longitud { get; set; }
    public string? Tipo { get; set; }
    [Display(Name = "Código Propietario")]
    public int PropietarioId { get; set; }
    public int? Estado { get; set; }
    public Decimal? Precio { get; set; }
    public String? Uso { get; set; }

    [ForeignKey(nameof(PropietarioId))]
    public Propietario? Duenio { get; set; }
    public override string ToString()
    {
        return $"Codigo: {Id}, Direccion: {Direccion}";
    }
    public Inmueble() { }
    public Inmueble(int id, string? direccion, int? ambientes, int? superficie, decimal? latitud, decimal? longitud, string? tipo, int propietarioId, int? estado, decimal? precio, string? uso, Propietario? duenio)
    {
        this.Id = id;
        this.Direccion = direccion;
        this.Ambientes = ambientes;
        this.Superficie = superficie;
        this.Latitud = latitud;
        this.Longitud = longitud;
        this.Tipo = tipo;
        this.PropietarioId = propietarioId;
        this.Estado = estado;
        this.Precio = precio;
        this.Uso = uso;
        this.Duenio = duenio;
    }
}
