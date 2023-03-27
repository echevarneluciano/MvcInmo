namespace MvcInmo.Models;

public class Inquilino
{
    public string? Nombre { get; set; }
    public int Id { get; set; }
    public string? Apellido { get; set; }
    public string? DNI { get; set; }
    public string? Telefono { get; set; }
    public Inquilino(string Nombre, string Apellido, string DNI, string Telefono)
    {
        this.Nombre = Nombre;
        this.Apellido = Apellido;
        this.DNI = DNI;
        this.Telefono = Telefono;
    }
    public Inquilino() { }
}
