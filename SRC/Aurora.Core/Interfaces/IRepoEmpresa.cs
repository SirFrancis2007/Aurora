namespace Aurora.Core.Interfaces;

public interface IRepoEmpresa : IRepoAlta<Empresa>, IRepoListado<Empresa>, IRepoDetalle<Empresa, uint>
{
    public Task EliminarEmpresa(int idempresa);
    //public Task AgregarAdministrador(Administrador xAdministrador);
    public Task EliminarAdministrador(int xidadministrador);
    public Task<IEnumerable<Pedido>> ObtenerPedidos(int xidEmpresa); 
}
