namespace Aurora.Core.Interfaces;

public interface IRepoVehiculo : IRepoAlta<Vehiculo>, IRepoDetalle<Vehiculo, int>, IRepoListado<Vehiculo>
{
    public Task<Boolean> EliminarVehiculo (int idVehiculo);
    public Task<IEnumerable<Pedido>> ListarPedidosAsignados(int vehiculoId);
    public Task<Boolean> CambiarEstado(int vehiculoId, bool disponible);
}
