using Microsoft.Data.Sqlite;

public class PresupuestoRepository
{

    private string cadenaConexion = "Data Source=Tienda_final.db;";
    public void Crear(Presupuesto presupuesto)
    {

        var queryPresupuesto = @"INSERT INTO Presupuestos (NombreDestinatario, FechaCreacion) VALUES (@NombreDestinatario, @FechaCreacion);";
        var queryDetalle = @"INSERT INTO PresupuestosDetalle (IdPresupuesto, IdProducto, Cantidad) VALUES (@IdPresupuesto, @IdProducto, @Cantidad)";

        using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
        {
            connection.Open();
            var commandPresupuesto = new SqliteCommand(queryPresupuesto, connection);
            commandPresupuesto.Parameters.AddWithValue("@NombreDestinatario", presupuesto.NombreDestinatario);
            commandPresupuesto.Parameters.AddWithValue("@FechaCreacion", presupuesto.FechaCreacion);
            commandPresupuesto.ExecuteNonQuery();
            var obtenerID = new SqliteCommand("SELECT last_insert_rowid();", connection); // Ojo aqui
            long idCreado = (long)obtenerID.ExecuteScalar();

            foreach (var DetalleProducto in presupuesto.Detalle)
            {
                var commandDetalle = new SqliteCommand(queryDetalle, connection);
                commandDetalle.Parameters.AddWithValue("@IdPresupuesto", idCreado);
                commandDetalle.Parameters.AddWithValue("@IdProducto", DetalleProducto.Producto.IdProducto);
                commandDetalle.Parameters.AddWithValue("@Cantidad", DetalleProducto.Cantidad);
                commandDetalle.ExecuteNonQuery();
            }
            connection.Close();
        }
    }

    public List<Presupuesto> Listar()
    {
        var listaPresupuesto = new List<Presupuesto>();
        var queryString = @"SELECT * FROM Presupuestos;";
        using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
        {
            connection.Open();
            using (SqliteCommand command = new SqliteCommand(queryString, connection))
            {


                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var presupuesto = new Presupuesto();
                        presupuesto.IdPresupuesto = Convert.ToInt32(reader["idPresupuesto"]);
                        presupuesto.NombreDestinatario = reader["NombreDestinatario"].ToString();
                        string FechaCreacion = reader["FechaCreacion"].ToString();
                        presupuesto.FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]);
                        presupuesto.Detalle = ObtenerDetalle(Convert.ToInt32(reader["idPresupuesto"]));
                        listaPresupuesto.Add(presupuesto);
                    }
                }
            }
            connection.Close();
        }
        return listaPresupuesto;
    }
    public List<PresupuestoDetalle>? ObtenerDetalle(int id)
    {
        var listaDetalles = new List<PresupuestoDetalle>();
        var queryString = @"SELECT * FROM PresupuestosDetalle WHERE idPresupuesto = @id;";
        using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@id", id);
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var productoRep = new ProductoRepository();
                    int idProducto = Convert.ToInt32(reader["idProducto"]);
                    var producto = productoRep.ObtenerDetalle(idProducto);
                    if (producto != null)
                    {
                        var detalle = new PresupuestoDetalle();
                        detalle.Cantidad = Convert.ToInt32(reader["Cantidad"]);
                        detalle.Producto = producto;
                        listaDetalles.Add(detalle);
                    }
                }

            }
            connection.Close();
        }
        return listaDetalles;
    }

    public bool agregarProductoAPresupuesto(int idPresupuesto, int idProducto, int cantidad)
    {
        var queryString = "INSERT INTO PresupuestosDetalle (IdPresupuesto, IdProducto, Cantidad) VALUES(@IdPresupuesto, @IdProducto, @Cantidad);";
        using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
        {
            connection.Open();
            var command = new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@IdPresupuesto", idPresupuesto);
            command.Parameters.AddWithValue("@IdProducto", idProducto);
            command.Parameters.AddWithValue("@Cantidad", cantidad);
            int chi = command.ExecuteNonQuery(); // guarda si afectó 1 o más fila o devuelve -1 si no.
            connection.Close();
            return chi > 0;
        }
        }

    public bool Eliminar(int id)
    {
        var queryString = "DELETE FROM Presupuestos WHERE idPresupuesto = @id;";
        var queryString2 = "DELETE FROM PresupuestosDetalle WHERE idPresupuesto = @id;";
        using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
        {
            connection.Open();
            // borro presupuesto detaie
            SqliteCommand command2 = new SqliteCommand(queryString2, connection);
            command2.Parameters.AddWithValue("@id", id);
            int exito2 = command2.ExecuteNonQuery();
            // borro presupuesto desp por la FK 
            SqliteCommand command = new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@id", id);
            int exito = command.ExecuteNonQuery();
            connection.Close();
            return exito > 0;
        }
    }

}