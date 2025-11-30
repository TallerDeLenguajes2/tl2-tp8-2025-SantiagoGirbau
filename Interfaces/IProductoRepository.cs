using System.Collections.Generic;
using tl2_tp8_2025_SantiagoGirbau.Models; // Asegúrate de tener los using correctos

namespace tl2_tp8_2025_SantiagoGirbau.Interfaces
{
    // CUMPLE DI: Abstracción del Repositorio de Productos
    public interface IProductoRepository
    {
        // El método GetAll devuelve una lista de Producto
        List<Producto> Listar();
        
        // El método GetById devuelve un único Producto o null
        Producto ObtenerPorId(int id);
        
        // El método Add recibe un Producto para dar de alta
        void Crear(Producto producto);
        
        // El método Update recibe un Producto para modificar
        void Modificar(Producto producto);
        
        // El método Delete recibe el ID del producto a eliminar
        void Eliminar(int id);
    }
}