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
            presupuestoRepository.Crear(presupuesto);  // actua en el repositorio
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
            var exito = presupuestoRepository.Eliminar(id);  // actua en el repositorio
            return RedirectToAction("Index"); // redirige a la vista de index 
        }

          [HttpGet]
        public IActionResult AgregarProductoADetalle(int id)
        {
            return View(id); // me lleva a la vista AgregarProductoADetalle
        }

        [HttpPost]
        public IActionResult AgregarProductoADetalle(PresupuestoDetalle Detalle, int id)
        {
            var presupuesto = presupuestoRepository.ObtenerPorId(id);
            presupuestoRepository.agregarProductoAPresupuesto(id, Detalle.Producto.IdProducto, Detalle.Cantidad);  // actua en el repositorio
            return RedirectToAction("Index"); // me lleva a la vista AgregarProductoADetalle
        }


        //    [HttpPost]
        // public IActionResult ConfirmarModificarPresupuesto(Presupuesto presupuesto)
        // {
        // presupuestoRepository.ModificarPresupuesto(presupuesto.IdPresupuesto, presupuesto);
            
        //     return RedirectToAction("Index"); // redirige a la vista de index 
        // }

    }
}