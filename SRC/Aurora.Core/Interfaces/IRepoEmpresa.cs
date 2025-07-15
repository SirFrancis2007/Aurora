namespace Aurora.Core.Interfaces;

public interface IRepoEmpresa : IRepoAlta<Empresa>, IRepoListado<Empresa>, IRepoDetalle<Empresa, uint>
{
    public Task EliminarEmpresaAsync(int idempresa);
    public Task EliminarAdministradorAsync(int xidadministrador);
    public Task<IEnumerable<(Pedido, Empresa)>> ObtenerPedidosAsync(int xidEmpresa); 
    public Task<Empresa?> ObtenerPorNombreAsync(string Nombre);
}
