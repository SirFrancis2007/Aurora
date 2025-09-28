namespace Aurora.Core.Interfaces;

public interface IRepoConductor : IRepoAlta<Conductor>, IRepoDetalle<Conductor, int>, IRepoListado<Conductor>
{
    /// <summary>
    /// Permite a un conductor iniciar sesión.
    /// </summary>
    /// <param name="nombre">Nombre del conductor.</param>
    /// <param name="contrasena">Contraseña del conductor.</param>
    /// <returns>El conductor autenticado, o null si las credenciales no son válidas.</returns>
    public Task<Conductor?> Loguearse(string nombre, string contrasena);

    /// <summary>
    /// Elimina un conductor según su Id.
    /// </summary>
    /// <param name="id">Identificador del conductor.</param>
    /// <returns>True si fue eliminado, False si no existe.</returns>
    public Task<bool> EliminarConductor(int id);

    /// <summary>
    /// Obtiene el listado de conductores disponibles (libres).
    /// </summary>
    /// <returns>Lista de conductores libres.</returns>
    public Task<List<Conductor>> ListarConductoresLibres();
}
