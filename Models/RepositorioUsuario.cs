using MySql.Data.MySqlClient;

namespace MvcInmo.Models;


public class RepositorioUsuario
{
    string connectionString = "Server=localhost;User=root;Password=;Database=inmobiliaria;SslMode=none";
    public RepositorioUsuario()
    {

    }

    public int Alta(Usuario e)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"INSERT INTO Usuarios 
					(Nombre, Apellido, Avatar, Email, Clave, Rol) 
					VALUES (@nombre, @apellido, @avatar, @email, @clave, @rol);
					SELECT LAST_INSERT_ID();";//devuelve el id insertado (LAST_INSERT_ID para mysql)
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                //command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@nombre", e.Nombre);
                command.Parameters.AddWithValue("@apellido", e.Apellido);
                if (String.IsNullOrEmpty(e.Avatar))
                    command.Parameters.AddWithValue("@avatar", DBNull.Value);
                else
                    command.Parameters.AddWithValue("@avatar", e.Avatar);
                command.Parameters.AddWithValue("@email", e.Email);
                command.Parameters.AddWithValue("@clave", e.Clave);
                command.Parameters.AddWithValue("@rol", e.Rol);
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
            string query = "DELETE FROM Usuarios WHERE Id = @id";
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
    public int Modificacion(Usuario e)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"UPDATE Usuarios 
					SET Nombre=@nombre, Apellido=@apellido, Avatar=@avatar, Email=@email, Clave=@clave, Rol=@rol
					WHERE Id = @id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                //command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@nombre", e.Nombre);
                command.Parameters.AddWithValue("@apellido", e.Apellido);
                command.Parameters.AddWithValue("@avatar", e.Avatar);
                command.Parameters.AddWithValue("@email", e.Email);
                command.Parameters.AddWithValue("@clave", e.Clave);
                command.Parameters.AddWithValue("@rol", e.Rol);
                command.Parameters.AddWithValue("@id", e.Id);
                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res;
    }

    public IList<Usuario> ObtenerTodos()
    {
        IList<Usuario> res = new List<Usuario>();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = @"SELECT Id, Nombre, Apellido, Avatar, Email, Clave, Rol
					FROM Usuarios";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                //command.CommandType = CommandType.Text;
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Usuario e = new Usuario
                    {
                        Id = reader.GetInt32(nameof(Usuario.Id)),
                        Nombre = reader.GetString(nameof(Usuario.Nombre)),
                        Apellido = reader.GetString(nameof(Usuario.Apellido)),
                        //Avatar = reader.GetString(nameof(Usuario.Avatar)),
                        Avatar = (reader.IsDBNull(3)) ? "" : reader.GetString(3),
                        Email = reader.GetString(nameof(Usuario.Email)),
                        Clave = reader.GetString(nameof(Usuario.Clave)),
                        Rol = reader.GetInt32(nameof(Usuario.Rol)),
                    };
                    res.Add(e);
                }
            }
            connection.Close();
        }
        return res;
    }

    public Usuario ObtenerPorId(int id)
    {
        Usuario? e = null;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"SELECT 
					Id, Nombre, Apellido, Avatar, Email, Clave, Rol 
					FROM Usuarios
					WHERE Id=@id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.Add("@id", MySqlDbType.Int16).Value = id;
                //command.CommandType = CommandType.Text;
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    e = new Usuario
                    {
                        Id = reader.GetInt32("Id"),
                        Nombre = reader.GetString("Nombre"),
                        Apellido = reader.GetString("Apellido"),
                        Avatar = reader.GetString("Avatar"),
                        Email = reader.GetString("Email"),
                        Clave = reader.GetString("Clave"),
                        Rol = reader.GetInt32("Rol"),
                    };
                }
                connection.Close();
            }
        }
        return e;
    }

    public Usuario ObtenerPorEmail(string email)
    {
        Usuario? e = null;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"SELECT
					Id, Nombre, Apellido, Avatar, Email, Clave, Rol FROM Usuarios
					WHERE Email=@email";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                //command.CommandType = CommandType.Text;
                command.Parameters.Add("@email", MySqlDbType.VarChar).Value = email;
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    e = new Usuario
                    {
                        Id = reader.GetInt32("Id"),
                        Nombre = reader.GetString("Nombre"),
                        Apellido = reader.GetString("Apellido"),
                        Avatar = reader.GetString("Avatar"),
                        Email = reader.GetString("Email"),
                        Clave = reader.GetString("Clave"),
                        Rol = reader.GetInt32("Rol"),
                    };
                }
                connection.Close();
            }
        }
        return e;
    }
}
