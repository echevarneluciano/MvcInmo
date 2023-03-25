using MySql.Data.MySqlClient;

namespace MvcInmo.Models;

public class RepositorioInquilino
{
    string connectionString = "Server=localhost;User=root;Password=;Database=inmobiliaria;SslMode=none";
    public RepositorioInquilino()
    {

    }
    public List<Inquilino> GetInquilinos()
    {
        List<Inquilino> Inquilinos = new List<Inquilino>();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = @"SELECT Id, Nombre, Apellido, DNI FROM Inquilinos";
            using (var command = new MySqlCommand(query, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Inquilino Inquilino = new Inquilino
                        {
                            Id = reader.GetInt32(nameof(Inquilino.Id)),
                            Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                            Apellido = reader.GetString(nameof(Inquilino.Apellido)),
                            DNI = reader.GetString(nameof(Inquilino.DNI)),
                        };
                        Inquilinos.Add(Inquilino);
                    }
                }
            }
            connection.Close();
        }
        return Inquilinos;
    }

    public int Alta(Inquilino Inquilino)
    {
        int res = 0;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"INSERT INTO Inquilinos (Nombre, Apellido, DNI)) 
            VALUES (@Nombre, @Apellido, @DNI);
            SELECT LAST_INSERT_ID();";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Nombre", Inquilino.Nombre);
                command.Parameters.AddWithValue("@Apellido", Inquilino.Apellido);
                command.Parameters.AddWithValue("@DNI", Inquilino.DNI);
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                Inquilino.Id = res;
                connection.Close();
            }
        }
        return res;
    }
}
