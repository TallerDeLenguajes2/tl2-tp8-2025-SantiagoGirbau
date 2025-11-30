using Microsoft.Data.Sqlite;
using tl2_tp8_2025_SantiagoGirbau.Interfaces;

public class ProductoRepository : IProductoRepository
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

    public void Modificar(Producto producto)
    {
        using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
        {
            var queryString = "UPDATE Productos SET Descripcion = @Descripcion, Precio = @Precio WHERE idProducto = @id;";
            SqliteCommand command = new SqliteCommand(queryString, connection);
            connection.Open();
            command.Parameters.Add(new SqliteParameter("@id", producto.IdProducto));
            command.Parameters.Add(new SqliteParameter("@Descripcion", producto.Descripcion));
            command.Parameters.Add(new SqliteParameter("@Precio", producto.Precio));
            var exito = command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public Producto? ObtenerPorId(int id)
    {
        var queryString = "SELECT * FROM Productos WHERE idProducto = @id;";
        Producto? producto = null;
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
                    producto.IdProducto = id;
                    producto.Descripcion = reader["Descripcion"].ToString();
                    producto.Precio = Convert.ToDouble(reader["Precio"]);
                }
            }
            connection.Close();
            return producto;
        }
    }
    public void Eliminar(int id)
    {
        var queryString = "DELETE FROM Productos WHERE idProducto = @id;";
        using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

}