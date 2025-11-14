public interface IRepository<T>
{
    public void Crear(T X);
    public List<T> Listar();
    public T ?ObtenerDetalle(int id);
    public bool Eliminar(int id);
}