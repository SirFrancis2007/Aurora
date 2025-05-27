namespace Aurora.Core.Interfaces;

public interface IRepoEmpresa : IRepoAlta<Empresa>, IRepoListado<Empresa>, IRepoDetalle<Empresa, uint>
{
    public void EliminarEmpresa(int idempresa);
    public IEnumerable<Pedido> ObtenerPedidos(int xidEmpresa); 
}
