using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_SantiagoGirbau.Interfaces;

namespace WebApplication1.Controllers
{

    public class ProductoController : Controller
    {
        private IProductoRepository _productoRepository;
        private IAuthenticationService _authService;

        public ProductoController(IProductoRepository prodRepo, IAuthenticationService authService)
        {
            _productoRepository = prodRepo;
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // ZONA DE SEGURIDAD //
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;
            // FIN ZONA DE SEGURIDAD //

            List<Producto> productos = _productoRepository.Listar();
            return View(productos);
        }

        [HttpGet]
        public IActionResult CrearProducto()
        {
            // ZONA DE SEGURIDAD //
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;
            // FIN ZONA DE SEGURIDAD //

            return View(); // me lleva a la vista de crearProducto (el formulario)
        }
        [HttpPost]
        public IActionResult CrearProducto(ProductoViewModel productoModel)
        {
            // ZONA DE SEGURIDAD //
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;
            // FIN ZONA DE SEGURIDAD //

            if (!ModelState.IsValid)
            {
                return View(productoModel);
            }

            var producto = new Producto
            {
                Descripcion = productoModel.Descripcion,
                Precio = productoModel.Precio
            };

            _productoRepository.Crear(producto);  // hace cosas en el repositorio
            return RedirectToAction("Index"); // redirige a la vista de index
        }

        [HttpGet]
        public IActionResult EliminarProducto(int id)
        {
            // ZONA DE SEGURIDAD //
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;
            // FIN ZONA DE SEGURIDAD //

            var producto = _productoRepository.ObtenerPorId(id);
            if (producto == null) return NotFound("no existe el producto");
            return View(producto); // me lleva a la vista EliminarProducto
        }
        [HttpGet]
        public IActionResult ConfirmarEliminarProducto(int id)
        {
            // ZONA DE SEGURIDAD //
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;
            // FIN ZONA DE SEGURIDAD //

            _productoRepository.Eliminar(id);  // hace cosas en el repositorio
            return RedirectToAction("Index"); // redirige a la vista de index 
        }

        [HttpGet]
        public IActionResult ModificarProducto(int id)
        {
            // ZONA DE SEGURIDAD //
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;
            // FIN ZONA DE SEGURIDAD //

            if (!ModelState.IsValid)
            {
                return View(id);
            }

            var producto = _productoRepository.ObtenerPorId(id); // hace cosas en el repositorio
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
            // ZONA DE SEGURIDAD //
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;
            // FIN ZONA DE SEGURIDAD //

            var producto = new Producto
            {
                IdProducto = productoModel.IdProducto,
                Descripcion = productoModel.Descripcion,
                Precio = productoModel.Precio
            };

            _productoRepository.Modificar(producto);

            return RedirectToAction("Index"); // redirige a la vista de index 
        }

        private IActionResult CheckAdminPermissions()
        {

            // 1. No logueado? -> vuelve al login
            if (!_authService.IsAuthenticated())
            {
                return RedirectToAction("Index", "Login");
            }

            // 2. No es Administrador? -> Da Error
            if (!_authService.HasAccessLevel("Administrador"))
            {
                // Llamamos a AccesoDenegado (llama a la vista correspondiente de Productos)
                return RedirectToAction(nameof(AccesoDenegado));
            }
            return null; // Permiso concedido
        }
        public IActionResult AccesoDenegado()
        {
            // El usuario está logueado, pero no tiene el rol suficiente.
            return View();
        }
        // resto del código con las correspondientes correcciones

    }
}