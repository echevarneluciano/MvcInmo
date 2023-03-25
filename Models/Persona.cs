namespace MvcInmo.Models;

public class Persona
{
    public string? Nombre { get; set; }
    public int Id { get; set; }
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
    public DateTime Nacimiento { get; set; }
    public Persona(string nombre = "")
    {
        Nombre = nombre;
    }
    public Persona() { }
}
