public class PedidoEmpresaDTO
{
    public int IdPedido { get; set; }
    public string NombrePedido { get; set; } = string.Empty;
    public double Peso { get; set; }
    public double Volumen { get; set; }
    public string Estado { get; set; } = string.Empty;
    public DateTime FechaDespacho { get; set; }

    public int IdEmpresa { get; set; }
    public string NombreEmpresa { get; set; } = string.Empty;
}
