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
            var query = @"SELECT c.Id, FechaInicio, FechaFin, c.Precio, InquilinoId, InmuebleId, 
            i.Nombre, i.Apellido, m.Tipo, m.PropietarioId, p.Nombre, p.Apellido, m.Direccion  
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
                                Direccion = reader.GetString(12),
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
            string query = @"INSERT INTO contratos (FechaInicio, FechaFin, Precio, InquilinoId, InmuebleId)
            VALUES (@fechaInicio, @fechaFin, @precio, @inquilinoId, @inmuebleId);
			SELECT LAST_INSERT_ID();";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.Add("@fechaInicio", MySqlDbType.DateTime).Value = entidad.FechaInicio;
                command.Parameters.Add("@fechaFin", MySqlDbType.DateTime).Value = entidad.FechaFin;
                command.Parameters.Add("@precio", MySqlDbType.Decimal).Value = entidad.Precio;
                command.Parameters.Add("@inquilinoId", MySqlDbType.Int32).Value = entidad.InquilinoId;
                command.Parameters.Add("@inmuebleId", MySqlDbType.Int32).Value = (entidad.Id == 0) ? entidad.InmuebleId : entidad.Id;
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
            string query = @$"SELECT c.Id, FechaInicio, FechaFin, c.Precio, InquilinoId, InmuebleId, 
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
            string query = @"UPDATE Contratos SET FechaInicio=@fechaInicio, FechaFin=@fechaFin, 
            Precio=@precio, InquilinoId=@inquilinoId, InmuebleId=@inmuebleId  
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

    public Contrato compruebaFechas(int idInmueble, DateTime fechaInicio, DateTime fechaFin)
    {
        Contrato p = null;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @$"SELECT c.Id FROM contratos c 
                           WHERE	c.InmuebleId=@id
                           AND (@fechaInicio BETWEEN c.FechaInicio AND c.FechaFin 
                           OR @fechaFin	 BETWEEN c.FechaInicio AND c.FechaFin);";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.Add("@id", MySqlDbType.Int16).Value = idInmueble;
                command.Parameters.Add("@fechaInicio", MySqlDbType.DateTime).Value = fechaInicio;
                command.Parameters.Add("@fechaFin", MySqlDbType.DateTime).Value = fechaFin;
                //command.CommandType = CommandType.Text;
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    p = new Contrato
                    {
                        Id = reader.GetInt32(0),
                    };
                }
                connection.Close();
            }
        }
        return p;
    }

    public IList<Contrato> GetContratosPorInmueble(int idInmueble)
    {
        List<Contrato> Contratos = new List<Contrato>();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = @$"SELECT c.Id, FechaInicio, FechaFin, c.Precio, InquilinoId, InmuebleId, 
            i.Nombre, i.Apellido, m.Tipo, m.PropietarioId, p.Nombre, p.Apellido, m.Direccion  
            FROM Contratos c 
            INNER JOIN Inquilinos i
				ON  c.InquilinoId = i.Id
            INNER JOIN inmuebles m 
				ON  c.InmuebleId = m.Id
            INNER JOIN propietarios p    
                ON m.PropietarioId = p.Id
            WHERE m.Id={idInmueble};";
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
                                Direccion = reader.GetString(12),
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

    public IList<Contrato> GetContratosVigentes(DateTime fechaInicio, DateTime fechaFin)
    {
        List<Contrato> Contratos = new List<Contrato>();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = @"SELECT c.Id, FechaInicio, FechaFin, c.Precio, InquilinoId, InmuebleId, 
            i.Nombre, i.Apellido, m.Tipo, m.PropietarioId, p.Nombre, p.Apellido, m.Direccion  
            FROM Contratos c 
            INNER JOIN Inquilinos i
				ON  c.InquilinoId = i.Id
            INNER JOIN inmuebles m 
				ON  c.InmuebleId = m.Id
            INNER JOIN propietarios p    
                ON m.PropietarioId = p.Id
            WHERE @fechaInicio BETWEEN c.FechaInicio AND c.FechaFin 
                OR @fechaFin	 BETWEEN c.FechaInicio AND c.FechaFin;";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.Add("@fechaInicio", MySqlDbType.DateTime).Value = fechaInicio;
                command.Parameters.Add("@fechaFin", MySqlDbType.DateTime).Value = fechaFin;
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
                                Direccion = reader.GetString(12),
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

}


