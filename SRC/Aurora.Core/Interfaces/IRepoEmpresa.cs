namespace Aurora.Core.Interfaces;

public interface IRepoEmpresa : IRepoAlta<Empresa>, IRepoListado<Empresa>, IRepoDetalle<Empresa, uint>
{
    public Task<bool> LoginAsync(string Nombre);
    public Task EliminarEmpresaAsync(int idempresa);
    public Task EliminarAdministradorAsync(int xidadministrador);
    public Task<IEnumerable<PedidoEmpresaDTO>> ObtenerPedidosAsync(int xidEmpresa);
    public Task<Empresa?> ObtenerPorNombreAsync(string Nombre);
    public Task<IEnumerable<Administrador>> ObtenerAdministradoresXempresaAsync(int xidEmpresa);
}
