using System.ComponentModel.DataAnnotations;

public class PresupuestoViewModel
{
    private int idPresupuesto;

    private string nombreDestinatario;



    private DateTime fechaCreacion;
    private List<PresupuestoDetalle> detalles;

    public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }

    [Display(Name = "Nombre o Email del Destinatario")]
    [Required(ErrorMessage = "El nombre o email es obligatorio.")]
    public string NombreDestinatario { get => nombreDestinatario; set => nombreDestinatario = value; }

    [Display(Name = "Fecha de Creación")]
    [Required(ErrorMessage = "La fecha es obligatoria.")]
    [DataType(DataType.Date)]
    public DateTime FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }
    public List<PresupuestoDetalle> Detalles { get => detalles; set => detalles = value; }
}