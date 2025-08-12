namespace MinimalAPI.DTO;

public record struct PedidoDTO
{
    public required string NombrePedido { get; init; }
    public required double Peso { get; init; }
    public required double Volumen { get; init; }
    public required string Estado { get; init; }
    public required DateTime FechaDespacho { get; init; }
}

