namespace Aurora.Core.Interfaces;

public interface IRepoRuta
{
    public void CrearRuta(Ruta NewRuta);
    public void EliminarRura(int idRuta);
    public Ruta ObtenerRutaPorId(int idRuta);
    public void ObtenerRutaPorCondicion();
    public List<Ruta> ListarRutas();
}
