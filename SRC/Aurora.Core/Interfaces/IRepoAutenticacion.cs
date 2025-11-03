namespace Aurora.Core.Interfaces;  

public interface IRepoAutenticacion
{
    string Rol { get; } 
    Task<bool> LoguearseAsync(string usuario, string contrasena);
    string ObtenerControladorRedireccion();
    string ObtenerAccionRedireccion();
}