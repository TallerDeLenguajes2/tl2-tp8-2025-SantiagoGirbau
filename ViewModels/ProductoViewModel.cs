using System.ComponentModel.DataAnnotations;
public class ProductoViewModel
{
    public int IdProducto { get; set; }

    [Required]
    [Display(Name = "Descripción del Producto")]
    [StringLength(250, ErrorMessage = "La descripción no puede superar los 250 caracteres.")]
    public string Descripcion { get; set; }
    
    [Display(Name = "Precio Unitario")]
    [Required(ErrorMessage = "El precio es obligatorio.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser un valor positivo.")]
    public double Precio { get; set; }
    public ProductoViewModel() { }
    public ProductoViewModel(Producto producto)
    {
        IdProducto = producto.IdProducto;
        Descripcion = producto.Descripcion;
        Precio = producto.Precio;
    }
}