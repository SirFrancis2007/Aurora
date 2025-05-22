namespace Aurora.Core.Interfaces;

public interface IRepoVehiculo : IRepoAlta<Vehiculo>, IRepoDetalle<Vehiculo, int>, IRepoListado<Vehiculo>
{
    public bool EliminarVehiculo (int idVehiculo);
    public List<Pedido> ListarPedidosAsignados(int vehiculoId);
    public bool CambiarEstado(int vehiculoId, bool disponible);
}
