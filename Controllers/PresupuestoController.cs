using Microsoft.AspNetCore.Mvc;
namespace WebApplication1.Controllers
{

    public class PresupuestoController : Controller
    {
        private PresupuestoRepository presupuestoRepository;
        private ProductoRepository productoRepository;
        public PresupuestoController()
        {
            presupuestoRepository = new PresupuestoRepository();
            productoRepository = new ProductoRepository();
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Presupuesto> presupuestos = presupuestoRepository.Listar();
            return View(presupuestos);
        }

        [HttpGet]
        public IActionResult DetallePresupuesto(int id)
        {
            var DetalleVM = new DetallePresupuestoViewModel();
            var presupuesto = new Presupuesto();
            presupuesto = presupuestoRepository.ObtenerPorId(id);
            DetalleVM.NombreDestinatario = presupuesto.NombreDestinatario;
            DetalleVM.IdPresupuesto = id;
            DetalleVM.Detalles = presupuestoRepository.ObtenerDetalle(id);
            return View(DetalleVM);
        }

         [HttpGet]
        public IActionResult CrearPresupuesto()
        {
            return View(); // me lleva a la vista de crearPresupuesto (el formulario)
        }
        [HttpPost]

        public IActionResult CrearPresupuesto(Presupuesto presupuesto)
        {
            presupuestoRepository.Crear(presupuesto);  // hace cosas en el repositorio
            return RedirectToAction("Index"); // redirige a la vista de index
        }

        [HttpGet]
        public IActionResult EliminarPresupuesto(int id)
        {
            var presupuesto = presupuestoRepository.ObtenerPorId(id);
            if (presupuesto == null) return NotFound("no existe el presupuesto");
            return View(presupuesto); // me lleva a la vista EliminarPresupuesto
        }
        [HttpGet]
        public IActionResult ConfirmarEliminarPresupuesto(int id)
        {
            var exito = presupuestoRepository.Eliminar(id);  // hace cosas en el repositorio
            return RedirectToAction("Index"); // redirige a la vista de index 
        }

        [HttpGet]
        public IActionResult AgregarProductoADetalle(int id)
        {
            var  DetalleVM = new AgregarDetalleViewModel();
            
            DetalleVM.TodosLosProductos = productoRepository.Listar();
            DetalleVM.IdPresupuesto = id;
            
            return View(DetalleVM); // me lleva a la vista AgregarProductoADetalle
        }

        [HttpPost]
        public IActionResult AgregarProductoADetalle(AgregarDetalleViewModel DetalleVM)
        {

            // Armo mi PresupuestoDetalle a partir de lo que tengo de DetalleVM
            var Detalle = new PresupuestoDetalle();
            var id = DetalleVM.IdPresupuesto;
            Detalle.Cantidad=DetalleVM.Cantidad;
            Detalle.Producto = productoRepository.ObtenerDetalle(DetalleVM.IdProducto);


            presupuestoRepository.agregarProductoAPresupuesto(Detalle, id); // hace cosas en el repositorio
              
            return RedirectToAction("DetallePresupuesto", new { id }); // me lleva a la vista AgregarProductoADetalle
        }

    }
}