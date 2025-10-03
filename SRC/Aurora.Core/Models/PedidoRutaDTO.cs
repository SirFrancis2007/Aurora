namespace Aurora.Core.Models;

public class PedidoRutaDTO
{
    public int idEmpresa { get; set; }
    public int IdPedido { get; set; }
    public string NombrePedido { get; set; } = string.Empty;
    public double Peso { get; set; }
    public double Volumen { get; set; }
    public string Estado { get; set; } = string.Empty;
    public DateTime FechaDespacho { get; set; }
    public string EmpresaOrigen { get; set; }
    public string EmpresaDestino { get; set; }
    public string Origen { get; set; }
    public string Destino { get; set; }
}
