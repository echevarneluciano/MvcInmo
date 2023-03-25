using MySql.Data.MySqlClient;

namespace MvcInmo.Models;

public class RepositorioPropietario
{
    string connectionString = "Server=localhost;User=root;Password=;Database=inmobiliaria;SslMode=none";
    public RepositorioPropietario()
    {

    }
    public List<Propietario> GetPropietarios()
    {
        List<Propietario> propietarios = new List<Propietario>();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = @"SELECT Id, Nombre, Apellido, DNI FROM propietarios";
            using (var command = new MySqlCommand(query, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Propietario propietario = new Propietario
                        {
                            Id = reader.GetInt32(nameof(Propietario.Id)),
                            Nombre = reader.GetString(nameof(Propietario.Nombre)),
                            Apellido = reader.GetString(nameof(Propietario.Apellido)),
                            DNI = reader.GetString(nameof(Propietario.DNI)),
                        };
                        propietarios.Add(propietario);
                    }
                }
            }
            connection.Close();
        }
        return propietarios;
    }

    public int Alta(Propietario Propietario)
    {
        int res = 0;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"INSERT INTO Propietarios (Nombre, Apellido, DNI)) 
            VALUES (@Nombre, @Apellido, @DNI);
            SELECT LAST_INSERT_ID();";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Nombre", Propietario.Nombre);
                command.Parameters.AddWithValue("@Apellido", Propietario.Apellido);
                command.Parameters.AddWithValue("@DNI", Propietario.DNI);
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                Propietario.Id = res;
                connection.Close();
            }
        }
        return res;
    }
}
