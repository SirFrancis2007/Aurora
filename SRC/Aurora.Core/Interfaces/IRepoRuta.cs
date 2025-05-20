namespace Aurora.Core.Interfaces;

public interface IRepoRuta : IRepoAlta<Ruta>, IRepoDetalle<Ruta, int>, IRepoListado<Ruta> 
{
    public Ruta ObtenerRutaPorCondicion(int? idRuta, string Origen, string Destino);
}
