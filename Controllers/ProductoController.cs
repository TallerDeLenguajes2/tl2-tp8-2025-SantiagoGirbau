using Microsoft.AspNetCore.Mvc;
namespace WebApplication1.Controllers
{

    public class ProductoController : Controller
    {
        private ProductoRepository productoRepository;
        public ProductoController()
        {
            productoRepository = new ProductoRepository();
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Producto> productos = productoRepository.Listar();
            return View(productos);
        }

         [HttpGet]
        public IActionResult CrearProducto()
        {
            return View(); // me lleva a la vista de crearProducto (el formulario)
        }
        [HttpPost]
        public IActionResult CrearProducto(Producto producto)
        {
            productoRepository.Crear(producto);  // actua en el repositorio
            return RedirectToAction("Index"); // redirige a la vista de index
        }

        [HttpGet]
        public IActionResult EliminarProducto(int id)
        {
            var producto = productoRepository.ObtenerDetalle(id);
            if (producto == null) return NotFound("no existe el producto");
            return View(producto); // me lleva a la vista EliminarProducto
        }
        [HttpGet]
        public IActionResult ConfirmarEliminarProducto(int id)
        {
            var exito = productoRepository.Eliminar(id);  // actua en el repositorio
            return RedirectToAction("Index"); // redirige a la vista de index 
        }

          [HttpGet]
        public IActionResult ModificarProducto(int id)
        {
            var producto = productoRepository.ObtenerDetalle(id);
            if (producto == null) return NotFound("no existe el producto");  // actua en el repositorio
            return View(producto); // me lleva a la vista ModificarProducto
        }
            [HttpPost]
        public IActionResult ConfirmarModificarProducto(Producto producto)
        {
        productoRepository.ModificarProducto(producto.IdProducto, producto);
            
            return RedirectToAction("Index"); // redirige a la vista de index 
        }

    }
}