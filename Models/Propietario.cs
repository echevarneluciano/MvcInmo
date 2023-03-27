namespace MvcInmo.Models;

public class Propietario
{
    public string? Nombre { get; set; }
    public int Id { get; set; }
    public string? Apellido { get; set; }
    public string? DNI { get; set; }
    public string? Telefono { get; set; }
    public Propietario(string Nombre, string Apellido, string DNI, string Telefono)
    {
        this.Nombre = Nombre;
        this.Apellido = Apellido;
        this.DNI = DNI;
        this.Telefono = Telefono;
    }
    public Propietario() { }
}
