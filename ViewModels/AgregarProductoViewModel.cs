using System.ComponentModel.DataAnnotations;
public class AgregarDetalleViewModel
{
    private List<Producto>? todosLosProductos;
    private int idPresupuesto;
    private int cantidad;
    private string nombreDestinatario;


    private int idProducto;

    [Display(Name = "Cantidad")]
    [Required(ErrorMessage = "La cantidad es obligatoria.")]
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero.")]
    public int Cantidad { get => cantidad; set => cantidad = value; }
    
    public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }
    public List<Producto>? TodosLosProductos { get => todosLosProductos; set => todosLosProductos = value; }


    [Display(Name = "Producto a agregar")]
    [Required(ErrorMessage = "Se debe elegir un producto.")]
    [Range(1, int.MaxValue, ErrorMessage = "Se debe elegir un producto.")]
    public int IdProducto { get => idProducto; set => idProducto = value; }
    public string NombreDestinatario { get => nombreDestinatario; set => nombreDestinatario = value; }
}