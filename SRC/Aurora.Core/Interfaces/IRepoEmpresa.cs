namespace Aurora.Core.Interfaces;

public interface IRepoEmpresa : IRepoAlta<Empresa>, IRepoListado<Empresa>, IRepoDetalle<Empresa, uint>
{
    public void EliminarEmpresa(int idempresa);
    //public void AgregarAdministrador(Administrador xAdministrador);
    //public void EliminarAdministrador(int xidadministrador);
    public IEnumerable<Pedido> ObtenerPedidos(int xidEmpresa); 
}
