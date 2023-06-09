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
            var query = @"SELECT Id, Nombre, Apellido, DNI, Telefono, Email FROM Inquilinos";
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
                            Telefono = reader.GetString(nameof(Inquilino.Telefono)),
                            Email = reader.GetString(nameof(Inquilino.Email)),
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
            string query = @"INSERT INTO Inquilinos (Nombre, Apellido, DNI, Telefono, Email)
            VALUES (@Nombre, @Apellido, @DNI, @Telefono, @Email);
            SELECT LAST_INSERT_ID(); ";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Nombre", Inquilino.Nombre);
                command.Parameters.AddWithValue("@Apellido", Inquilino.Apellido);
                command.Parameters.AddWithValue("@DNI", Inquilino.DNI);
                command.Parameters.AddWithValue("@Telefono", Inquilino.Telefono);
                command.Parameters.AddWithValue("@Email", Inquilino.Email);
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                Inquilino.Id = res;
                connection.Close();
            }
        }
        return res;
    }

    public Inquilino GetInquilino(int id)
    {
        Inquilino p = null;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"SELECT Id, Nombre, Apellido, Dni, Telefono, Email 
					FROM Inquilinos
					WHERE Id=@id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.Add("@id", MySqlDbType.Int16).Value = id;
                //command.CommandType = CommandType.Text;
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    p = new Inquilino
                    {
                        Id = reader.GetInt32(nameof(Inquilino.Id)),
                        Nombre = reader.GetString("Nombre"),
                        Apellido = reader.GetString("Apellido"),
                        DNI = reader.GetString("DNI"),
                        Telefono = reader.GetString("Telefono"),
                        Email = reader.GetString("Email"),
                    };
                }
                connection.Close();
            }
        }
        return p;
    }

    public int Modificacion(Inquilino p)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"UPDATE Inquilinos 
					SET Nombre=@nombre, Apellido=@apellido, DNI=@DNI, Telefono=@telefono, Email=@email 
					WHERE Id = @id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                // command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@nombre", p.Nombre);
                command.Parameters.AddWithValue("@apellido", p.Apellido);
                command.Parameters.AddWithValue("@DNI", p.DNI);
                command.Parameters.AddWithValue("@telefono", p.Telefono);
                command.Parameters.AddWithValue("@email", p.Email);
                command.Parameters.AddWithValue("@id", p.Id);
                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res;
    }

    public int Baja(int id)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "DELETE FROM Inquilinos WHERE Id = @id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                //command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res;
    }

}
