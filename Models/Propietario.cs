using System.ComponentModel.DataAnnotations;

namespace MvcInmo.Models;

public class Propietario
{
    public string? Nombre { get; set; }
    [Display(Name = "Código")]
    public int Id { get; set; }
    public string? Apellido { get; set; }
    public string? DNI { get; set; }
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public string? Clave { get; set; }
    public string? Avatar { get; set; }
    public Propietario(string Nombre, string Apellido, string DNI, string Telefono, string Email, string Clave, string Avatar)
    {
        this.Nombre = Nombre;
        this.Apellido = Apellido;
        this.DNI = DNI;
        this.Telefono = Telefono;
        this.Email = Email;
        this.Clave = Clave;
        this.Avatar = Avatar;
    }
    public Propietario() { }
    public override string ToString()
    {
        //return $"{Apellido}, {Nombre}";
        return $"{Nombre} {Apellido}";
    }
}
