public class PresupuestoDetalle
{
    private int idPresupuesto;
    private Producto producto;
    private int cantidad;

    public Producto Producto { get => producto; set => producto = value; }
    public int Cantidad { get => cantidad; set => cantidad = value; }
    public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }

}