namespace MinimalAPI.DTO;

public record struct EmpresaDTO
{
    public string Nombre { get; init; }
    public string Contrasena { get; init; }
}

public record struct EmpresaPedidoDTO
{
    public EmpresaPedidoDTO()
    {
    }

    public uint IdEmpresa { get; init; }
    public string Nombre { get; init; }
    public List<PedidoDTO> Pedidos { get; init; } = new();
}