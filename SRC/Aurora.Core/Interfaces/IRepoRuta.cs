namespace Aurora.Core.Interfaces;

public interface IRepoRuta : IRepoAlta<Ruta>, IRepoDetalle<Ruta, int>, IRepoListado<Ruta> 
{
    public Task<Ruta> ObtenerRutaPorCondicionAsync(int? idRuta, string Origen, string Destino);
}
