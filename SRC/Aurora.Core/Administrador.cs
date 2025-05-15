namespace Aurora.Core;

public class Administrador
{
    public int IdAdministrador { get; set; }
    public uint IdEmpresa {get; set;}
    public required string Nombre { get; set; }
    public required string Password { get; set; }
    public List<Pedido>? PedidoXAdmnistrador {get; set;}
}
