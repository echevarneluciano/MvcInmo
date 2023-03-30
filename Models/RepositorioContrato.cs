using MySql.Data.MySqlClient;

namespace MvcInmo.Models;

public class RepositorioContrato
{
    string connectionString = "Server=localhost;User=root;Password=;Database=inmobiliaria;SslMode=none";
    public RepositorioContrato()
    {

    }
    public IList<Contrato> GetContratos()
    {
        List<Contrato> Contratos = new List<Contrato>();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = @"SELECT c.Id, FechaInicio, FechaFin, Precio, InquilinoId, InmuebleId, 
            i.Nombre, i.Apellido, m.Tipo, m.PropietarioId, p.Nombre, p.Apellido  
            FROM Contratos c 
            INNER JOIN Inquilinos i
				ON  c.InquilinoId = i.Id
            INNER JOIN inmuebles m 
				ON  c.InmuebleId = m.Id
            INNER JOIN propietarios p    
                ON m.PropietarioId = p.Id;";
            using (var command = new MySqlCommand(query, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Contrato Contrato = new Contrato
                        {
                            Id = reader.GetInt32(0),
                            FechaInicio = reader.GetDateTime(1),
                            FechaFin = reader.GetDateTime(2),
                            Precio = reader.GetDecimal(3),
                            InquilinoId = reader.GetInt32(4),
                            InmuebleId = reader.GetInt32(5),
                            inquilino = new Inquilino
                            {
                                Id = reader.GetInt32(4),
                                Nombre = reader.GetString(6),
                                Apellido = reader.GetString(7),
                            },
                            inmueble = new Inmueble
                            {
                                Id = reader.GetInt32(5),
                                Tipo = reader.GetString(8),
                                PropietarioId = reader.GetInt32(9),
                            },
                            propietario = new Propietario
                            {
                                Id = reader.GetInt32(9),
                                Nombre = reader.GetString(10),
                                Apellido = reader.GetString(11),
                            }
                        };
                        Contratos.Add(Contrato);
                    }
                }
            }
            connection.Close();
        }
        return Contratos;
    }

    public int Alta(Contrato entidad)
    {
        int res = 0;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"INSERT INTO Contratos 
					(FechaInicio, FechaFin, Precio, InquilinoId, InmuebleId)
					VALUES (@fechaInicio, @fechaFin, @precio, @inquilinoId, @inmuebleId);
					SELECT LAST_INSERT_ID();";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.Add("@fechaInicio", MySqlDbType.DateTime).Value = entidad.FechaInicio;
                command.Parameters.Add("@fechaFin", MySqlDbType.DateTime).Value = entidad.FechaFin;
                command.Parameters.Add("@precio", MySqlDbType.Decimal).Value = entidad.Precio;
                command.Parameters.Add("@inquilinoId", MySqlDbType.Int16).Value = entidad.InquilinoId;
                command.Parameters.Add("@inmuebleId", MySqlDbType.Int16).Value = entidad.InmuebleId;
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                entidad.Id = res;
                connection.Close();
            }
        }
        return res;
    }

    public Contrato GetContrato(int id)
    {
        Contrato p = null;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @$"SELECT c.Id, FechaInicio, FechaFin, Precio, InquilinoId, InmuebleId, 
            i.Nombre, i.Apellido, m.Tipo, m.PropietarioId  
            FROM Contratos c 
            INNER JOIN Inquilinos i
				ON  c.InquilinoId = i.Id
            INNER JOIN inmuebles m 
				ON  c.InmuebleId = m.Id
			WHERE c.Id=@id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.Add("@id", MySqlDbType.Int16).Value = id;
                //command.CommandType = CommandType.Text;
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    p = new Contrato
                    {
                        Id = reader.GetInt32(0),
                        FechaInicio = reader.GetDateTime(1),
                        FechaFin = reader.GetDateTime(2),
                        Precio = reader.GetDecimal(3),
                        InquilinoId = reader.GetInt32(4),
                        InmuebleId = reader.GetInt32(5),
                        inquilino = new Inquilino
                        {
                            Nombre = reader.GetString(6),
                            Apellido = reader.GetString(7),
                        },
                        inmueble = new Inmueble
                        {
                            Tipo = reader.GetString(8),
                            PropietarioId = reader.GetInt32(9),
                        }
                    };
                }
                connection.Close();
            }
        }
        return p;
    }

    public int Modificacion(Contrato entidad)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"UPDATE Contratos SET (FechaInicio, FechaFin, Precio, InquilinoId, InmuebleId)
					VALUES (@fechaInicio, @fechaFin, @precio, @inquilinoId, @inmuebleId); 
            WHERE Id = @id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@fechaInicio", entidad.FechaInicio);
                command.Parameters.AddWithValue("@fechaFin", entidad.FechaFin);
                command.Parameters.AddWithValue("@precio", entidad.Precio);
                command.Parameters.AddWithValue("@inquilinoId", entidad.InquilinoId);
                command.Parameters.AddWithValue("@inmuebleId", entidad.InmuebleId);
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
            string query = "DELETE FROM Contratos WHERE Id = @id";
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


