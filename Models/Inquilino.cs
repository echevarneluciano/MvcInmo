using System.ComponentModel.DataAnnotations;

namespace MvcInmo.Models;

public class Inquilino
{
    public string? Nombre { get; set; }
    [Display(Name = "Código")]
    public int Id { get; set; }
    public string? Apellido { get; set; }
    [Display(Name = "DNI Inquilino")]
    public string? DNI { get; set; }
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public Inquilino(string Nombre, string Apellido, string DNI, string Telefono, string Email)
    {
        this.Nombre = Nombre;
        this.Apellido = Apellido;
        this.DNI = DNI;
        this.Telefono = Telefono;
        this.Email = Email;
    }
    public Inquilino() { }
    public override string ToString()
    {
        return $"{Nombre} {Apellido}";
    }
}
