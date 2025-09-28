namespace Aurora.Core.Interfaces;

public interface IRepoHisrorialPedido
{
    // Aclaracion! Este procedimiento ya lo realica el trigger, asi que es redundante.
    //public void RegistrarCambio(int pedidoId, string estadoAnterior, string estadoNuevo, DateTime fechaCambio);
    public Task<List<HistorialPedido>> ObtenerHistorialPorPedidoAsync(int pedidoId);

    /// <summary>
    /// Obtiene los historiales de pedidos relacionados a una empresa,
    /// ya sea como origen o destino.
    /// </summary>
    /// <param name="idEmpresa">Identificador de la empresa.</param>
    /// <returns>Lista de historiales asociados a la empresa.</returns>
    public Task<IEnumerable<PedidoEmpresaDTO>> ObtenerHistorialesDeEmpresa(int idEmpresa);

    /// <summary>
    /// Obtiene el detalle de un historial específico.
    /// </summary>
    /// <param name="idEmpresa">Identificador del historial.</param>
    /// <returns>El historial solicitado, o null si no existe.</returns>
    public Task<IEnumerable<HistorialDTO>> ObtenerHistorialCompleto(int idEmpresa);
}
