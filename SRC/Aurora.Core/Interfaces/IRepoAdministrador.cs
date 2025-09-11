namespace Aurora.Core.Interfaces;

public interface IRepoAdministrador : IRepoAlta<Administrador>, IRepoDetalle<Administrador, int>, IRepoListado<Administrador>
{
    Task<Boolean> LoginAsync(string nombre, string password);
    Task<IEnumerable<Administrador>> ObtenerDataXidAsync(int ID);

    Task<bool> UpdateAdministrador(Administrador administrador);
}
