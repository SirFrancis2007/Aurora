namespace Aurora.Core.Interfaces;

public interface IRepoVehiculo : IRepoAlta<Vehiculo>, IRepoDetalle<Vehiculo, int>, IRepoListado<Vehiculo>
{
    public void EliminarVehiculo (int idVehiculo);
    public List<Pedido> ListarPedidosAsignados(int vehiculoId);
    public void CambiarEstado(int vehiculoId, bool disponible);
}
