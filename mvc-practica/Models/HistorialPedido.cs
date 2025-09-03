namespace Aurora.Core;

public class HistorialPedido
{
    public int IdHistorial { get; set; }
    public int XidPedido { get; set; }
    public required string EstadoAnterior { get; set; }
    public required string EstadoActual { get; set; }
    public required DateOnly FechaCambio { get; set; }
}
