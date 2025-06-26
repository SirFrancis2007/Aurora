namespace Aurora.Core.Interfaces;

public interface IRepoPedido : IRepoAlta<Pedido>, IRepoDetalle<Pedido, int>, IRepoListado<Pedido> 
{
    public Task<Pedido>? ObtenerPedidoXCondicion(DateTime Xfecha);
    public Task ActualizarEstado(int pedidoId, string nuevoEstado);
    //public Task AsignarVehiculo(int pedidoId, int vehiculoId); 
    // No hay metodo asignarvehiculo ya que al crear el pedido ya se pide asociarlo con un vehiculo.
}
