using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_SantiagoGirbau.Interfaces;

namespace WebApplication1.Controllers
{

    public class ProductoController : Controller
    {
        private IProductoRepository productoRepository;
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
        public IActionResult CrearProducto(ProductoViewModel productoModel)
        {

            if (!ModelState.IsValid)
            {
                return View(productoModel);
            }

            var producto = new Producto
            {
                Descripcion = productoModel.Descripcion,
                Precio = productoModel.Precio
            };

            productoRepository.Crear(producto);  // hace cosas en el repositorio
            return RedirectToAction("Index"); // redirige a la vista de index
        }

        [HttpGet]
        public IActionResult EliminarProducto(int id)
        {
            var producto = productoRepository.ObtenerPorId(id);
            if (producto == null) return NotFound("no existe el producto");
            return View(producto); // me lleva a la vista EliminarProducto
        }
        [HttpGet]
        public IActionResult ConfirmarEliminarProducto(int id)
        {
            productoRepository.Eliminar(id);  // hace cosas en el repositorio
            return RedirectToAction("Index"); // redirige a la vista de index 
        }

          [HttpGet]
        public IActionResult ModificarProducto(int id)
        {
              if (!ModelState.IsValid)
            {
                return View(id);
            }

            var producto = productoRepository.ObtenerPorId(id); // hace cosas en el repositorio
            var productoVM = new ProductoViewModel();
            if (producto == null) return NotFound("no existe el producto");  

            productoVM.Descripcion = producto.Descripcion;
            productoVM.IdProducto = producto.IdProducto;
            productoVM.Precio = producto.Precio;

            return View(productoVM); // me lleva a la vista ModificarProducto
        }
            [HttpPost]
        public IActionResult ConfirmarModificarProducto(ProductoViewModel productoModel)
        {
              var producto = new Producto
            {
                IdProducto = productoModel.IdProducto,
                Descripcion = productoModel.Descripcion,
                Precio = productoModel.Precio
            };
            
        productoRepository.Modificar(producto);
            
            return RedirectToAction("Index"); // redirige a la vista de index 
        }

    }
}