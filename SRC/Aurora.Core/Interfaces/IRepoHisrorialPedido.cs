namespace Aurora.Core.Interfaces;

public interface IRepoHisrorialPedido
{
    public void RegistrarCambio(int pedidoId, string estadoAnterior, string estadoNuevo, DateTime fechaCambio);
    public List<HistorialPedido> ObtenerHistorialPorPedido(int pedidoId);
}
