namespace Aurora.Core.Interfaces;

public interface IRepoPedido : IRepoAlta<Pedido>, IRepoDetalle<Pedido, int>, IRepoListado<Pedido> 
{
    public IEnumerable<Pedido> ObtenerPedidoXCondicion();
    public void ActualizarEstado(int pedidoId, string nuevoEstado);
    public void AsignarVehiculo(int pedidoId, int vehiculoId);
}
