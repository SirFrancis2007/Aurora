namespace Aurora.Core.Interfaces;

public interface IRepoAdministrador : IRepoAlta<Administrador>, IRepoDetalle<Administrador, int>, IRepoListado<Administrador> 
{
    public Task<Pedido> ObtenerPedidoXAdmin(int idadministrador);
}
