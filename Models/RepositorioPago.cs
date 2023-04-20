using MySql.Data.MySqlClient;

namespace MvcInmo.Models;


public class RepositorioPago
{
    string connectionString = "Server=localhost;User=root;Password=;Database=inmobiliaria;SslMode=none";
    public RepositorioPago()
    {

    }

    public int Alta(Pago e)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"INSERT INTO Pagos 
					(Mes, FechaPagado, Importe, ContratoId) 
					VALUES (@mes, @fechapagado, @importe, @contratoid);
					SELECT LAST_INSERT_ID();";//devuelve el id insertado (LAST_INSERT_ID para mysql)
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                //command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@mes", e.Mes);
                command.Parameters.AddWithValue("@fechapagado", e.FechaPagado);
                command.Parameters.AddWithValue("@importe", e.Importe);
                command.Parameters.AddWithValue("@contratoid", e.ContratoId);
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                e.Id = res;
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
            string query = "DELETE FROM Pagos WHERE Id = @id";
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
    public int Modificacion(Pago e)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"UPDATE Pagos 
					SET Mes = @mes, FechaPagado = @fechapagado, 
                    ContratoId = @contratoid, Importe = @importe  
					WHERE Id = @id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                //command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@mes", e.Mes);
                command.Parameters.AddWithValue("@fechapagado", e.FechaPagado);
                command.Parameters.AddWithValue("@contratoid", e.ContratoId);
                command.Parameters.AddWithValue("@importe", e.Importe);
                command.Parameters.AddWithValue("@id", e.Id);
                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res;
    }

    public IList<Pago> ObtenerTodos()
    {
        IList<Pago> res = new List<Pago>();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = @"SELECT p.Id, Mes, FechaPagado, ContratoId, Importe, 
                        c.Id, c.Precio, c.InquilinoId, i.Id, i.DNI   
                        FROM Pagos p
                        INNER JOIN Contratos c 
                        ON c.Id = p.ContratoId
                        INNER	JOIN	inquilinos i
                        ON	c.InquilinoId=i.Id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                //command.CommandType = CommandType.Text;
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Pago e = new Pago
                    {
                        Id = reader.GetInt32(0),
                        Mes = reader.GetInt32(1),
                        FechaPagado = (reader.IsDBNull(2)) ? null : reader.GetDateTime(2),
                        ContratoId = reader.GetInt32(3),
                        Importe = (reader.IsDBNull(4)) ? 0 : reader.GetDecimal(4),
                        contrato = new Contrato()
                        {
                            Id = reader.GetInt32(5),
                            Precio = reader.GetDecimal(6),
                        },
                        inquilino = new Inquilino()
                        {
                            Id = reader.GetInt32(8),
                            DNI = reader.GetString(9),
                        }
                    };
                    res.Add(e);
                }
            }
            connection.Close();
        }
        return res;
    }

    public Pago ObtenerPorId(int id)
    {
        Pago? e = null;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"SELECT 
					p.Id, Mes, FechaPagado, ContratoId, Importe, 
                    c.Id, c.Precio, c.InquilinoId, i.Id, i.DNI   
					FROM Pagos p
                    INNER JOIN Contratos c 
                    ON c.Id = p.ContratoId
                    INNER JOIN Inquilinos i ON c.InquilinoId=i.Id
					WHERE p.Id=@id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.Add("@id", MySqlDbType.Int16).Value = id;
                //command.CommandType = CommandType.Text;
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    e = new Pago
                    {
                        Id = reader.GetInt32(0),
                        Mes = reader.GetInt32(1),
                        FechaPagado = (reader.IsDBNull(2)) ? null : reader.GetDateTime(2),
                        ContratoId = reader.GetInt32(3),
                        Importe = (reader.IsDBNull(4)) ? 0 : reader.GetDecimal(4),
                        contrato = new Contrato()
                        {
                            Id = reader.GetInt32(5),
                            Precio = reader.GetDecimal(6),
                        },
                        inquilino = new Inquilino()
                        {
                            Id = reader.GetInt32(8),
                            DNI = reader.GetString(9),
                        }
                    };
                }
                connection.Close();
            }
        }
        return e;
    }

    public IList<Pago> ObtenerPorContrato(int contratoid)
    {
        IList<Pago> res = new List<Pago>();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = @"SELECT p.Id, Mes, FechaPagado, ContratoId, Importe, 
                  c.Id, c.Precio, c.InquilinoId, i.Id, i.DNI   
                        FROM Pagos p
                        INNER JOIN Contratos c 
                        ON c.Id = ContratoId
                        INNER	JOIN	inquilinos i
                        ON	c.InquilinoId=i.Id
					         WHERE ContratoId=@contratoid";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                //command.CommandType = CommandType.Text;
                command.Parameters.Add("@contratoid", MySqlDbType.Int16).Value = contratoid;
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Pago e = new Pago
                    {
                        Id = reader.GetInt32(0),
                        Mes = reader.GetInt32(1),
                        FechaPagado = (reader.IsDBNull(2)) ? null : reader.GetDateTime(2),
                        ContratoId = reader.GetInt32(3),
                        Importe = (reader.IsDBNull(4)) ? 0 : reader.GetDecimal(4),
                        contrato = new Contrato()
                        {
                            Id = reader.GetInt32(5),
                            Precio = reader.GetDecimal(6),
                        },
                        inquilino = new Inquilino()
                        {
                            Id = reader.GetInt32(8),
                            DNI = reader.GetString(9),
                        }
                    };
                    res.Add(e);
                }
            }
            connection.Close();
        }
        return res;
    }

    public int ModificacionPorContratoId(int idContrato, DateTime fechaTerminado)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"UPDATE Pagos 
					SET FechaPagado = @fechaTerminado     
					WHERE ContratoId = @idContrato";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                //command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@idContrato", idContrato);
                command.Parameters.AddWithValue("@fechaTerminado", fechaTerminado);
                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res;
    }

    public Double ObtenerDeuda(int idContrato)
    {
        Double res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"SELECT SUM(Precio) - SUM(Importe) AS Deuda FROM pagos JOIN contratos 
                ON ContratoId= contratos.Id
                WHERE contratos.Id=@idContrato
                AND FechaFin<CURDATE()";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                //command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@idContrato", idContrato);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    res = (reader.IsDBNull(0)) ? 0 : reader.GetDouble(0);
                }
                connection.Close();
            }
        }
        return res;
    }

}
