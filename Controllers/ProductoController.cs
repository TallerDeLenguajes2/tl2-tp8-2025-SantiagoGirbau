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
    }
}