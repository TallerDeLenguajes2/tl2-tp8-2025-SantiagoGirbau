using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_SantiagoGirbau.Interfaces;

namespace WebApplication1.Controllers
{

    public class PresupuestoController : Controller
    {
        private readonly IPresupuestoRepository _presupuestoRepository;
        private readonly IProductoRepository _productoRepository;
        private IAuthenticationService _authService;
        public PresupuestoController(IPresupuestoRepository IPresRepo, IProductoRepository IProdRepo, IAuthenticationService IAuthServ)
        {
            _presupuestoRepository = IPresRepo;
            _productoRepository = IProdRepo;
            _authService = IAuthServ;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // ZONA DE SEGURIDAD //
            var securityCheck = CheckClientPermissions();
            if (securityCheck != null) return securityCheck;
            // FIN ZONA DE SEGURIDAD //

            List<Presupuesto> presupuestos = _presupuestoRepository.Listar();
            return View(presupuestos);
        }

        [HttpGet]
        public IActionResult DetallePresupuesto(int id)
        {
             // ZONA DE SEGURIDAD //
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;
            // FIN ZONA DE SEGURIDAD //
            
            var DetalleVM = new DetallePresupuestoViewModel();
            var presupuesto = new Presupuesto();
            
            presupuesto = _presupuestoRepository.ObtenerPorId(id);
            DetalleVM.NombreDestinatario = presupuesto.NombreDestinatario;
            DetalleVM.IdPresupuesto = id;
            DetalleVM.Detalles = _presupuestoRepository.ObtenerDetalle(id);
            return View(DetalleVM);
        }

         [HttpGet]
        public IActionResult CrearPresupuesto()
        {
             // ZONA DE SEGURIDAD //
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;
            // FIN ZONA DE SEGURIDAD //
            return View(); // me lleva a la vista de crearPresupuesto (el formulario)
        }
        [HttpPost]

        public IActionResult CrearPresupuesto(PresupuestoViewModel presupuestoVM)
        {  
             // ZONA DE SEGURIDAD //
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;
            // FIN ZONA DE SEGURIDAD //

               if (!ModelState.IsValid)
            {
                return View(presupuestoVM);
            }

            var presupuesto = new Presupuesto();
            presupuesto.NombreDestinatario = presupuestoVM.NombreDestinatario;
            presupuesto.IdPresupuesto = presupuestoVM.IdPresupuesto;
            presupuesto.FechaCreacion = presupuestoVM.FechaCreacion;
            presupuesto.Detalles = presupuestoVM.Detalles;

            _presupuestoRepository.Crear(presupuesto);  // hace cosas en el repositorio
            return RedirectToAction("Index"); // redirige a la vista de index
        }

        [HttpGet]
        public IActionResult EliminarPresupuesto(int id)
        {
             // ZONA DE SEGURIDAD //
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;
            // FIN ZONA DE SEGURIDAD //
            var presupuestoVM = new PresupuestoViewModel();
            var presupuesto = _presupuestoRepository.ObtenerPorId(id);

            presupuestoVM.Detalles = presupuesto.Detalles;
            presupuestoVM.IdPresupuesto = presupuesto.IdPresupuesto;
            presupuestoVM.NombreDestinatario = presupuesto.NombreDestinatario;
            if (presupuesto == null) return NotFound("no existe el presupuesto");
            return View(presupuestoVM); // me lleva a la vista EliminarPresupuesto
        }
        [HttpGet]
        public IActionResult ConfirmarEliminarPresupuesto(int id)
        {
             // ZONA DE SEGURIDAD //
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;
            // FIN ZONA DE SEGURIDAD //
            _presupuestoRepository.Eliminar(id);  // hace cosas en el repositorio
            return RedirectToAction("Index"); // redirige a la vista de index 
        }

        [HttpGet]
        public IActionResult AgregarProductoADetalle(int id)
        {
             // ZONA DE SEGURIDAD //
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;
            // FIN ZONA DE SEGURIDAD //
            var  DetalleVM = new AgregarDetalleViewModel();
            
            DetalleVM.TodosLosProductos = _productoRepository.Listar();
            DetalleVM.IdPresupuesto = id;
            DetalleVM.NombreDestinatario = _presupuestoRepository.ObtenerPorId(id).NombreDestinatario;
            
            return View(DetalleVM); // me lleva a la vista AgregarProductoADetalle
        }

        [HttpPost]
        public IActionResult AgregarProductoADetalle(AgregarDetalleViewModel DetalleVM)
        {
             // ZONA DE SEGURIDAD //
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;
            // FIN ZONA DE SEGURIDAD //
              if (!ModelState.IsValid)
            {
                DetalleVM.TodosLosProductos = _productoRepository.Listar();
                return View(DetalleVM);
            }
            
            // Armo mi PresupuestoDetalle a partir de lo que tengo de DetalleVM
            var Detalle = new PresupuestoDetalle();
            var id = DetalleVM.IdPresupuesto;
            Detalle.Cantidad=DetalleVM.Cantidad;
            Detalle.Producto = _productoRepository.ObtenerPorId(DetalleVM.IdProducto);


            _presupuestoRepository.AgregarDetalle(Detalle, id); // hace cosas en el repositorio
              
            return RedirectToAction("DetallePresupuesto", new { id }); // me lleva a la vista AgregarProductoADetalle
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
        private IActionResult CheckClientPermissions()
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
                if (!_authService.HasAccessLevel("Cliente"))
            {
                return RedirectToAction(nameof(AccesoDenegado));
            }
            }
            return null; // Permiso concedido
        }
        public IActionResult AccesoDenegado()
        {
            // El usuario está logueado, pero no tiene el rol suficiente.
            return View();
        }
    }
}