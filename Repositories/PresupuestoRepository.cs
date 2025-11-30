using Microsoft.Data.Sqlite;
using tl2_tp8_2025_SantiagoGirbau.Interfaces;

public class PresupuestoRepository : IPresupuestoRepository
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
            if (presupuesto.Detalles == null) return;
            foreach (var DetalleProducto in presupuesto.Detalles)
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
                        presupuesto.Detalles = ObtenerDetalle(Convert.ToInt32(reader["idPresupuesto"]));
                        listaPresupuesto.Add(presupuesto);
                    }
                }
            }
            connection.Close();
        }
        return listaPresupuesto;
    }

    public Presupuesto? ObtenerPorId(int id)
    {
        Presupuesto presupuesto = null;
        var queryString = @"SELECT * FROM Presupuestos WHERE idPresupuesto = @id;";
        using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@id", id);
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    presupuesto = new Presupuesto();
                    presupuesto.IdPresupuesto = Convert.ToInt32(reader["idPresupuesto"]);
                    presupuesto.NombreDestinatario = reader["NombreDestinatario"].ToString();
                    presupuesto.FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]);
                    presupuesto.Detalles = ObtenerDetalle(id);
                }
            }
            connection.Close();
        }
        return presupuesto;
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
                    var producto = productoRep.ObtenerPorId(idProducto);
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


    public void Actualizar(Presupuesto presupuesto)
    {
        const string sql = "UPDATE Presupuestos SET NombreDestinatario = @NombreDestinatario, FechaCreacion = @FechaCreacion WHERE IdPresupuesto = @Id";

        using var conexion = new SqliteConnection(cadenaConexion);
        conexion.Open();

        using var comando = new SqliteCommand(sql, conexion);
        comando.Parameters.AddWithValue("@NombreDestinatario", presupuesto.NombreDestinatario);
        comando.Parameters.AddWithValue("@FechaCreacion", presupuesto.FechaCreacion);
        comando.Parameters.AddWithValue("@Id", presupuesto.IdPresupuesto);

        comando.ExecuteNonQuery();
    }
    public void AgregarDetalle(PresupuestoDetalle detalle, int idPresupuesto)

    {
        int idProducto = detalle.Producto.IdProducto;
        int cantidad = detalle.Cantidad;
        var queryString = "INSERT INTO PresupuestosDetalle (IdPresupuesto, IdProducto, Cantidad) VALUES(@IdPresupuesto, @IdProducto, @Cantidad);";
        using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
        {
            connection.Open();
            var command = new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@IdPresupuesto", idPresupuesto);
            command.Parameters.AddWithValue("@IdProducto", idProducto);
            command.Parameters.AddWithValue("@Cantidad", cantidad);
            int exito = command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public void Eliminar(int id)
    {
        var queryString = "DELETE FROM Presupuestos WHERE idPresupuesto = @id;";
        var queryString2 = "DELETE FROM PresupuestosDetalle WHERE idPresupuesto = @id;";
        using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
        {
            connection.Open();
            // borro presupuesto detalle

            SqliteCommand command2 = new SqliteCommand(queryString2, connection);
            command2.Parameters.AddWithValue("@id", id);
            int exito2 = command2.ExecuteNonQuery();
            // borro presupuesto desp por la FK 
            SqliteCommand command = new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@id", id);
            int exito = command.ExecuteNonQuery();
            connection.Close();
        }
    }

}