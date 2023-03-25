using MySql.Data.MySqlClient;

namespace MvcInmo.Models;

public class RepositorioPersona
{
    string connectionString = "Server=localhost;User=root;Password=;Database=test;SslMode=none";
    public RepositorioPersona()
    {

    }
    public List<Persona> GetPersonas()
    {
        List<Persona> personas = new List<Persona>();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = @"SELECT Id, Nombre, Direccion, Telefono, Nacimiento FROM personas";
            using (var command = new MySqlCommand(query, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Persona persona = new Persona
                        {
                            Id = reader.GetInt32(nameof(Persona.Id)),
                            Nombre = reader.GetString(nameof(Persona.Nombre)),
                            Direccion = reader.GetString(nameof(Persona.Direccion)),
                            Telefono = reader.GetString(nameof(Persona.Telefono)),
                            Nacimiento = reader.GetDateTime(nameof(Persona.Nacimiento))
                        };
                        personas.Add(persona);
                    }
                }
            }
            connection.Close();
        }
        return personas;
    }

    public int Alta(Persona persona)
    {
        int res = 0;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"INSERT INTO personas (Nombre, Direccion, Telefono, Nacimiento) 
            VALUES (@Nombre, @Direccion, @Telefono, @Nacimiento);
            SELECT LAST_INSERT_ID();";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Nombre", persona.Nombre);
                command.Parameters.AddWithValue("@Direccion", persona.Direccion);
                command.Parameters.AddWithValue("@Telefono", persona.Telefono);
                command.Parameters.AddWithValue("@Nacimiento", persona.Nacimiento);
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                persona.Id = res;
                connection.Close();
            }
        }
        return res;
    }
}
