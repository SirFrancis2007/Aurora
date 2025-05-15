namespace Aurora.Core;

public class Pedido
{
    public int IdPedido { get; set; }
    public int XidAdministrador { get; set; }
    public int XidRuta { get; set; }
    public int XidEmpresa { get; set; }
    public required string NombrePedido { get; set; }
    public required double Peso { get; set; }
    public required double Volumen { get; set; }
    public required string Estado { get; set; }
    public required DateTime FechaDespacho { get; set; }
}
