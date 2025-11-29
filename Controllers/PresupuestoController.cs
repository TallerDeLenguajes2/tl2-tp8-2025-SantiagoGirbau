using Microsoft.AspNetCore.Mvc;
namespace WebApplication1.Controllers
{

    public class PresupuestoController : Controller
    {
        private PresupuestoRepository presupuestoRepository;
        public PresupuestoController()
        {
            presupuestoRepository = new PresupuestoRepository();
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
            List<PresupuestoDetalle> Detalles = presupuestoRepository.ObtenerDetalle(id);
            return View(Detalles);
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
            var  Detalle = new PresupuestoDetalle();
            Detalle.IdPresupuesto = id;
            
            return View(Detalle); // me lleva a la vista AgregarProductoADetalle
        }

        [HttpPost]
        public IActionResult AgregarProductoADetalle(PresupuestoDetalle Detalle, int id)
        {
            var presupuesto = presupuestoRepository.ObtenerPorId(id);
            presupuestoRepository.agregarProductoAPresupuesto(Detalle);
              // hace cosas en el repositorio
            return RedirectToAction("Index"); // me lleva a la vista AgregarProductoADetalle
        }


        //     [HttpPost]
        //  public IActionResult ConfirmarModificarPresupuesto(PresupuestoDetalle DetalleAgregar)
        //  {
            
        //  presupuestoRepository.agregarProductoAPresupuesto;
            
        //      return RedirectToAction("Index"); // redirige a la vista de index 
        //  }

    }
}