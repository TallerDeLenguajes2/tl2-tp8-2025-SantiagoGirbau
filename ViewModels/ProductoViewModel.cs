public class ProductoViewModel
{
    public int IdProducto { get; set; }
    public string Descripcion { get; set; }
    public double Precio { get; set; }
    public ProductoViewModel() { }
    public ProductoViewModel(Producto producto)
    {
        IdProducto = producto.IdProducto;
        Descripcion = producto.Descripcion;
        Precio = producto.Precio;
    }
}