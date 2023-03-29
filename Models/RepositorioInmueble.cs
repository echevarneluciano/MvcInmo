using MySql.Data.MySqlClient;

namespace MvcInmo.Models;

public class RepositorioInmueble
{
    string connectionString = "Server=localhost;User=root;Password=;Database=inmobiliaria;SslMode=none";
    public RepositorioInmueble()
    {

    }
    public IList<Inmueble> GetInmuebles()
    {
        List<Inmueble> Inmuebles = new List<Inmueble>();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = @"SELECT i.Id, Direccion, Ambientes, Superficie, Latitud, Longitud, PropietarioId, p.Nombre, p.Apellido 
            FROM Inmuebles i INNER JOIN Propietarios p ON i.PropietarioId = p.Id";
            using (var command = new MySqlCommand(query, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Inmueble Inmueble = new Inmueble
                        {
                            Id = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            Ambientes = reader.GetInt32(2),
                            Superficie = reader.GetInt32(3),
                            Latitud = reader.GetDecimal(4),
                            Longitud = reader.GetDecimal(5),
                            PropietarioId = reader.GetInt32(6),
                            Duenio = new Propietario
                            {
                                Id = reader.GetInt32(6),
                                Nombre = reader.GetString(7),
                                Apellido = reader.GetString(8),
                            }
                        };
                        Inmuebles.Add(Inmueble);
                    }
                }
            }
            connection.Close();
        }
        return Inmuebles;
    }

    public int Alta(Inmueble entidad)
    {
        int res = 0;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"INSERT INTO Inmuebles 
					(Direccion, Ambientes, Superficie, Latitud, Longitud, PropietarioId)
					VALUES (@direccion, @ambientes, @superficie, @latitud, @longitud, @propietarioId);
					SELECT SCOPE_IDENTITY();";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@direccion", entidad.Direccion);
                command.Parameters.AddWithValue("@ambientes", entidad.Ambientes);
                command.Parameters.AddWithValue("@superficie", entidad.Superficie);
                command.Parameters.AddWithValue("@latitud", entidad.Latitud);
                command.Parameters.AddWithValue("@longitud", entidad.Longitud);
                command.Parameters.AddWithValue("@propietarioId", entidad.PropietarioId);
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                entidad.Id = res;
                connection.Close();
            }
        }
        return res;
    }

    public Inmueble GetInmueble(int id)
    {
        Inmueble p = null;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @$"SELECT {nameof(Inmueble.Id)}, Direccion, Ambientes, Superficie, Latitud, Longitud, PropietarioId, 
                    p.Nombre, p.Apellido FROM Inmuebles i 
                    JOIN Propietarios p ON i.PropietarioId = p.Id
					WHERE {nameof(Inmueble.Id)}=@id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.Add("@id", MySqlDbType.Int16).Value = id;
                //command.CommandType = CommandType.Text;
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    p = new Inmueble
                    {
                        Id = reader.GetInt32(nameof(Inmueble.Id)),
                        Direccion = reader.GetString("Direccion"),
                        Ambientes = reader.GetInt32("Ambientes"),
                        Superficie = reader.GetInt32("Superficie"),
                        Latitud = reader.GetDecimal("Latitud"),
                        Longitud = reader.GetDecimal("Longitud"),
                        PropietarioId = reader.GetInt32("PropietarioId"),
                        Duenio = new Propietario
                        {
                            Id = reader.GetInt32("Id"),
                            Nombre = reader.GetString("Nombre"),
                            Apellido = reader.GetString("Apellido"),
                        }
                    };
                }
                connection.Close();
            }
        }
        return p;
    }

    public int Modificacion(Inmueble entidad)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"UPDATE Inmuebles SET Direccion=@direccion, Ambientes=@ambientes, 
            Superficie=@superficie, Latitud=@latitud, Longitud=@longitud, PropietarioId=@propietarioId 
            WHERE Id = @id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@direccion", entidad.Direccion);
                command.Parameters.AddWithValue("@ambientes", entidad.Ambientes);
                command.Parameters.AddWithValue("@superficie", entidad.Superficie);
                command.Parameters.AddWithValue("@latitud", entidad.Latitud);
                command.Parameters.AddWithValue("@longitud", entidad.Longitud);
                command.Parameters.AddWithValue("@propietarioId", entidad.PropietarioId);
                command.Parameters.AddWithValue("@id", entidad.Id);
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
            string query = "DELETE FROM Inmuebles WHERE Id = @id";
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

    public IList<Inmueble> BuscarPorPropietario(int idPropietario)
    {
        List<Inmueble> res = new List<Inmueble>();
        Inmueble entidad = null;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @$"
					SELECT {nameof(Inmueble.Id)}, Direccion, Ambientes, Superficie, Latitud, Longitud, PropietarioId, p.Nombre, p.Apellido
					FROM Inmuebles i JOIN Propietarios p ON i.PropietarioId = p.IdPropietario
					WHERE PropietarioId=@idPropietario";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.Add("@idPropietario", MySqlDbType.Int32).Value = idPropietario;
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    entidad = new Inmueble
                    {
                        Id = reader.GetInt32(nameof(Inmueble.Id)),
                        Direccion = reader.GetString("Direccion"),
                        Ambientes = reader.GetInt32("Ambientes"),
                        Superficie = reader.GetInt32("Superficie"),
                        Latitud = reader.GetDecimal("Latitud"),
                        Longitud = reader.GetDecimal("Longitud"),
                        PropietarioId = reader.GetInt32("PropietarioId"),
                        Duenio = new Propietario
                        {
                            Id = reader.GetInt32("Id"),
                            Nombre = reader.GetString("Nombre"),
                            Apellido = reader.GetString("Apellido"),
                        }
                    };
                    res.Add(entidad);
                }
                connection.Close();
            }
        }
        return res;
    }
}


