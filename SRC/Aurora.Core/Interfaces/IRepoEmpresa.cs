namespace Aurora.Core.Interfaces;

public interface IRepoEmpresa : IRepoAlta<Empresa>, IRepoListado<Empresa>, IRepoDetalle<Empresa, uint>
{
    public Task<bool> LoginAsync(string Nombre);
    public Task EliminarEmpresaAsync(int idempresa);
    public Task EliminarAdministradorAsync(int xidadministrador);
    public Task<IEnumerable<PedidoEmpresaDTO>> ObtenerPedidosAsync(int xidEmpresa);
    public Task<Empresa?> ObtenerPorNombreAsync(string Nombre); //Obtiene el id de la empresa a partir del nombre
    public Task<IEnumerable<Administrador>> ObtenerAdministradoresXempresaAsync(int xidEmpresa); //Obtiene los administradores de una empresa
    public Task<IEnumerable<Conductor>> ObtenerConductoresXempresaAsync(int xidEmpresa); //Obtiene los conductores de una empresa
}
