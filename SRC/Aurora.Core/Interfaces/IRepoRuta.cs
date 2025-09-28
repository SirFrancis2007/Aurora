namespace Aurora.Core.Interfaces;

public interface IRepoRuta : IRepoAlta<Ruta>, IRepoDetalle<Ruta, int>, IRepoListado<Ruta> 
{
    /// <summary>
    /// Obtiene una ruta según parámetros de origen, destino e identificador.
    /// </summary>
    /// <param name="origen">Ciudad o punto de origen.</param>
    /// <param name="destino">Ciudad o punto de destino.</param>
    /// <param name="idRuta">Identificador de la ruta.</param>
    /// <returns>La ruta encontrada, o null si no existe.</returns>
    public Task<Ruta?> ObtenerRutaPorParametrosAsync(string origen, string destino, int idRuta);
}
