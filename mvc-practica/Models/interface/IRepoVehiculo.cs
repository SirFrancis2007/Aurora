namespace Aurora.Core.Interfaces;

public interface IRepoVehiculo : IRepoAlta<Vehiculo>, IRepoDetalle<Vehiculo, int>, IRepoListado<Vehiculo>
{
    public Task<Boolean> EliminarVehiculoAsync(int idVehiculo);
    public Task<IEnumerable<Pedido>> ListarPedidosAsignadosAsync(int vehiculoId);
    public Task<Boolean> CambiarEstadoAsync(int vehiculoId, bool disponible);
}
