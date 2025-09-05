namespace Aurora.Core.Interfaces;

public interface IRepoAdministrador : IRepoAlta<Administrador>, IRepoDetalle<Administrador, int>, IRepoListado<Administrador>
{
    public Task LoguearseAsync(string nombre);
    Task<IEnumerable<Administrador>> ObtenerDataXidAsync(int ID);
    Task<bool> UpdateAdministrador(Administrador administrador);
}
