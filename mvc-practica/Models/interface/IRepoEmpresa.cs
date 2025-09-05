namespace Aurora.Core.Interfaces;

public interface IRepoEmpresa : IRepoAlta<Empresa>, IRepoListado<Empresa>, IRepoDetalle<Empresa, uint>
{
    public Task LoguearseAsync(string nombre);
    public Task RegistroAsync(string nombre);
    public Task RegistroAdministradorAsync(Administrador administrador);
    public Task EliminarEmpresaAsync(int idempresa);
    public Task EliminarAdministradorAsync(int xidadministrador);
    public Task<IEnumerable<PedidoEmpresaDTO>> ObtenerPedidosAsync(int xidEmpresa); 
    public Task<Empresa?> ObtenerPorNombreAsync(string Nombre);
}
