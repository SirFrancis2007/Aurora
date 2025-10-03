namespace Aurora.Core.Interfaces;

public interface IRepoAdministrador : IRepoAlta<Administrador>, IRepoDetalle<Administrador, int>, IRepoListado<Administrador>
{
    /// <summary>
    /// Permite a un administrador iniciar sesión.
    /// </summary>
    /// <param name="nombre">Nombre del administrador.</param>
    /// <param name="contrasena">Contraseña del administrador.</param>
    /// <returns>El administrador autenticado, o null si las credenciales no son válidas.</returns>
    Task<bool> LoguearseAsync(string nombre, string contrasena);

    /// <summary>
    /// Elimina un administrador por su Id.
    /// </summary>
    /// <param name="id">Identificador del administrador.</param>
    /// <returns>True si fue eliminado, false si no existe.</returns>
    Task<bool> EliminarAsync(int id);

    /// <summary>
    /// Obtiene la lista de administradores asociados a una empresa.
    /// </summary>
    /// <param name="idEmpresa">Identificador de la empresa.</param>
    /// <returns>Lista de administradores de esa empresa.</returns>
    Task<List<Administrador>> ObtenerPorEmpresaAsync(int idEmpresa);

    /// <summary>
    /// Actualizar datos de un administrador
    /// </summary>
    /// <param name="administrador"></param>
    /// <returns></returns>
    Task<bool> UpdateAdministrador(Administrador administrador);
    
    /// <summary>
    /// Obtener credenciales segun nombre y contraseña
    /// </summary>
    /// <param name="administrador"></param>
    /// <returns></returns>
    Task<Administrador> ObtenerCredenciales(string nombreAdministrador);
}
