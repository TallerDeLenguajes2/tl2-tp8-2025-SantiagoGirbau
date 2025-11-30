public class Presupuesto
{
    private int idPresupuesto;
    private string nombreDestinatario;
    private DateTime fechaCreacion;
    private List<PresupuestoDetalle> detalles;

    public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }
    public string NombreDestinatario { get => nombreDestinatario; set => nombreDestinatario = value; }
    public DateTime FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }
    public List<PresupuestoDetalle> Detalles { get => detalles; set => detalles = value; }

    public double MontoPresupuesto()
    {
        double monto = 0;

        foreach (var detalleProducto in detalles)
        {
            var producto = detalleProducto.Producto;
            monto += producto.Precio * detalleProducto.Cantidad;
        }

        return monto;
    }
    public double MontoPresupuestoConIva()
    {
        return MontoPresupuesto() * 1.21;
    } //considerar iva 21
    public int CantidadProductos()
    {
         int cant = 0;

        foreach (var detalleProducto in detalles)
        {
            cant += detalleProducto.Cantidad;
        }

        return cant;
    } //contar total de productos (sumador d todas las cantidades del detalle)
}