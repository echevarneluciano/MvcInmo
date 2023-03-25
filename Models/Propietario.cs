namespace MvcInmo.Models;

public class Propietario
{
    public string? Nombre { get; set; }
    public int Id { get; set; }
    public string? Apellido { get; set; }
    public string? DNI { get; set; }
    public Propietario(string Nombre, string Apellido, string DNI)
    {
        this.Nombre = Nombre;
        this.Apellido = Apellido;
        this.DNI = DNI;
    }
    public Propietario() { }
}
