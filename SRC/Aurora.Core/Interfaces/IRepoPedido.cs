using Aurora.Core.Models;

namespace Aurora.Core.Interfaces;

public interface IRepoPedido : IRepoAlta<Pedido>, IRepoDetalle<Pedido, int>, IRepoListado<Pedido>
{
    /// <summary>
    /// Actualiza el estado de un pedido según la acción de un administrador.
    /// </summary>
    /// <param name="idPedido">Identificador del pedido.</param>
    /// <param name="nuevoEstado">Nuevo estado del pedido.</param>
    /// <returns>True si se actualizó, False en caso contrario.</returns>
    public Task<bool> ActualizarEstadoPedidoPorAdmin(int idPedido, string nuevoEstado);

    /// <summary>
    /// Actualiza el estado de un pedido según la acción de un conductor.
    /// </summary>
    /// <param name="idPedido">Identificador del pedido.</param>
    /// <param name="nuevoEstado">Nuevo estado del pedido.</param>
    /// <returns>True si se actualizó, False en caso contrario.</returns>
    public Task<bool> ActualizarEstadoPedidoPorConductor(int idPedido, string nuevoEstado);

    /// <summary>
    /// Obtiene la lista de pedidos asociados a una empresa.
    /// </summary>
    /// <param name="idEmpresa">Identificador de la empresa.</param>
    /// <returns>Lista de pedidos de la empresa.</returns>
    public Task<List<PedidoEmpresaDTO>> ObtenerPedidosPorEmpresa(int idEmpresa);

    /// <summary>
    /// Obtiene la lista de pedidos asociados a una empresa en el estado RECIBIDO. Bandeja de Entrada Administradores
    /// </summary>
    /// <param name="idEmpresa">Identificador de la empresa.</param>
    /// <returns>Lista de pedidos de la empresa.</returns>
    public Task<List<PedidoRutaDTO>> ObtenerPedidosEmpresa(int idEmpresa);

    /// <summary>
    /// Obtiene la lista de pedidos asociados a una vehiculo. Bandeja de Entrada de conductor
    /// </summary>
    /// <param name="idVehiculo">Identificador del vehiculo.</param>
    /// <returns>Lista de pedidos del vehiculo.</returns>
    public Task<List<PedidoRutaDTO>> ObtenerPedidosPorVehiculo(int idVehiculo);
}
