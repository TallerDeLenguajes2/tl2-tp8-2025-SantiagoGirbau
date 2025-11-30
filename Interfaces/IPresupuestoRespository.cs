    // CUMPLE DI: Abstracción para el Repositorio de Presupuestos
    public interface IPresupuestoRepository
    {
        List<Presupuesto> Listar();
        
        Presupuesto ObtenerPorId(int id);

        List<PresupuestoDetalle>? ObtenerDetalle(int id);
        
        void Crear(Presupuesto presupuesto);
        
        void Actualizar(Presupuesto presupuesto);
        
        void Eliminar(int id);
        
        // Método clave del TP para la relación N:M
        void AgregarDetalle(PresupuestoDetalle detalle, int idPresupuesto);
    }