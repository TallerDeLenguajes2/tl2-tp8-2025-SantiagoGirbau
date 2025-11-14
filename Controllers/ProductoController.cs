using Microsoft.AspNetCore.Mvc;
namespace WebApplication1.Controllers
{

    public class ProductosController : Controller
    {
        private ProductoRepository productoRepository;
        public ProductosController()
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
            return View(id); // me lleva a la vista EliminarProducto
        }
        [HttpGet]
        public IActionResult ConfirmarEliminarProducto(int id)
        {
            var exito = productoRepository.Eliminar(id);  // actua en el repositorio
            return RedirectToAction("Index"); // redirige a la vista de index 
        }
    }
}