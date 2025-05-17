namespace Aurora.Core.Interfaces;

public interface IRepoHisrorialPedido
{
    // Aclaracion! Este procedimiento ya lo realica el trigger, asi que es redundante.
    //public void RegistrarCambio(int pedidoId, string estadoAnterior, string estadoNuevo, DateTime fechaCambio);
    public List<HistorialPedido> ObtenerHistorialPorPedido(int pedidoId);
}
