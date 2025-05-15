namespace Aurora.Core.Interfaces;

public interface IRepoVehiculo
{
    public void AñadirVehiculo (Vehiculo NewVehiculo);
    public void EliminarVehiculo (int idVehiculo);
    public List<Pedido> ListarPedidosAsignados(int vehiculoId);
    public void CambiarEstado(int vehiculoId, bool disponible);
}
