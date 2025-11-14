using Microsoft.Data.Sqlite;

public class ProductoRepository : IRepository<Producto>
{
    private string cadenaConexion = "Data Source=Tienda_final.db;";
    public void Crear(Producto producto)
    {

        var queryString = @"INSERT INTO Productos (Descripcion, Precio) VALUES (@Descripcion, @Precio);";
        using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
        {
            connection.Open();
            var command = new SqliteCommand(queryString, connection);
            command.Parameters.Add(new SqliteParameter("@Descripcion", producto.Descripcion));
            command.Parameters.Add(new SqliteParameter("@Precio", producto.Precio));

            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public List<Producto> Listar()
    {
        var queryString = @"SELECT * FROM Productos;";
        var listaProductos = new List<Producto>();
        using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
        {
            SqliteCommand command = new SqliteCommand(queryString, connection);
            connection.Open();
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var producto = new Producto();
                    producto.IdProducto = Convert.ToInt32(reader["idProducto"]);
                    producto.Descripcion = reader["Descripcion"].ToString();
                    producto.Precio = Convert.ToDouble(reader["Precio"]);
                    listaProductos.Add(producto);
                }
            }
            connection.Close(); //Preguntar si esto hace falta
        }
        return listaProductos;
    }

    public bool ModificarProducto(int id, string Descripcion)
    {
        using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
        {
            var queryString = "UPDATE Productos SET Descripcion = @Descripcion WHERE idProducto = @id;";
            SqliteCommand command = new SqliteCommand(queryString, connection);
            connection.Open();
            command.Parameters.Add(new SqliteParameter("@id", id));
            command.Parameters.Add(new SqliteParameter("@Descripcion", Descripcion));

            if (command.ExecuteNonQuery() == 0)
            {
                connection.Close();
                return false;
            }
            else
            {
                connection.Close();
                return true;
            }
        }
    }

    public Producto? ObtenerDetalle(int id)
    {
        var queryString = "SELECT * FROM Productos WHERE idProducto = @id;";
        Producto ?producto = null;
        using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@id", id);
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    producto = new Producto();
                    producto.IdProducto = Convert.ToInt32(reader["idProducto"]);
                    producto.Descripcion = reader["Descripcion"].ToString();
                    producto.Precio = Convert.ToDouble(reader["Precio"]);
                }
            }
            connection.Close();
            return producto;
        }
    }
    public bool Eliminar(int id)
    {
        var queryString = "DELETE FROM Productos WHERE idProducto = @id;";
        using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@id", id);
            int exito = command.ExecuteNonQuery();
            connection.Close();
            return exito > 0;
        }
    }

}