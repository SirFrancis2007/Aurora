using System.Data;
using Aurora.Core;
using Aurora.Core.Interfaces;

namespace Aurora.Dapper.ADO;

public class RepoPedido : RepoGenerico, IRepoPedido
{
    public RepoPedido(IDbConnection conexion) : base(conexion)
    {
    }

    public void ActualizarEstado(int pedidoId, string nuevoEstado)
    {
        throw new NotImplementedException();
    }

    public void Alta(Pedido elemento)
    {
        throw new NotImplementedException();
    }

    public void AsignarVehiculo(int pedidoId, int vehiculoId)
    {
        throw new NotImplementedException();
    }

    public Pedido? Detalle(int indiceABuscar)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Pedido> Obtener()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Pedido> ObtenerPedidoXCondicion()
    {
        throw new NotImplementedException();
    }
}
