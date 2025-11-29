public class DetallePresupuestoViewModel
{
    private List<PresupuestoDetalle> detalles;
    private int idPresupuesto;

    private string nombreDestinatario;

    public List<PresupuestoDetalle> Detalles { get => detalles; set => detalles = value; }
    public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }
    public string NombreDestinatario { get => nombreDestinatario; set => nombreDestinatario = value; }
}