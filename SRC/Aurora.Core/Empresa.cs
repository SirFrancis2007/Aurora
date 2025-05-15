namespace Aurora.Core;

public class Empresa
{
    public uint IdEmpresa {get; set;}
    public required string Nombre { get; set; }
    public List<Pedido>? PedidoxEmpresa {get; set;}
}
