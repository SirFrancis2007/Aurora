namespace MinimalAPI.DTO;

public record struct PedidoDTO
{
    public required string NombrePedido { get; init; }
    public required double Peso { get; init; }
    public required double Volumen { get; init; }
    public required string Estado { get; init; }
    public required DateTime FechaDespacho { get; init; }
}

public record struct PedidoEmpresaDTO
{
    public PedidoEmpresaDTO()
    {
    }

    public int IdPedido { get; set; }
    public string NombrePedido { get; set; } = string.Empty;
    public double Peso { get; set; }
    public double Volumen { get; set; }
    public string Estado { get; set; } = string.Empty;
    public DateTime FechaDespacho { get; set; }

    public int IdEmpresa { get; set; }
    public string NombreEmpresa { get; set; } = string.Empty;
}


