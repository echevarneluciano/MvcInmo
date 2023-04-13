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
					(Mes, FechaPagado, ContratoId) 
					VALUES (@mes, @fechapagado, @contratoid);
					SELECT LAST_INSERT_ID();";//devuelve el id insertado (LAST_INSERT_ID para mysql)
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                //command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@mes", e.Mes);
                command.Parameters.AddWithValue("@fechapagado", e.FechaPagado);
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
            var query = @"SELECT 
					p.Id, Mes, FechaPagado, ContratoId, Importe, 
                    c.Id, c.Precio   
					FROM Pagos p
                    INNER JOIN Contratos c 
                    ON c.Id = p.ContratoId";
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
                    c.Id, c.Precio   
					FROM Pagos p
                    INNER JOIN Contratos c 
                    ON c.Id = p.ContratoId
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
            var query = @"SELECT Id, Mes, FechaPagado, ContratoId, Importe 
					FROM Pagos WHERE ContratoId=@contratoid";
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
                        Id = reader.GetInt32(nameof(Pago.Id)),
                        Mes = reader.GetInt32(nameof(Pago.Mes)),
                        FechaPagado = reader.GetDateTime(nameof(Pago.FechaPagado)),
                        ContratoId = reader.GetInt32(nameof(Pago.ContratoId)),
                        Importe = reader.GetDecimal(nameof(Pago.Importe)),
                    };
                    res.Add(e);
                }
            }
            connection.Close();
        }
        return res;
    }

}
