using System.Data;
using Aurora.Core;
using Aurora.Core.Interfaces;
using Dapper;

namespace Aurora.Dapper.ADO;

public class RepoPedido : RepoGenerico, IRepoPedido
{
    public RepoPedido(IDbConnection conexion) : base(conexion)
    {
    }

    public void ActualizarEstado(int pedidoId, string nuevoEstado)
    {
        var parametetros = new DynamicParameters();
        parametetros.Add("xidPedido",pedidoId);
        parametetros.Add("xNuevoEstado",nuevoEstado);
        try
        {
            Conexion.Execute("SPUpdateEstadoPedido", parametetros);
        }
        catch (System.Exception)
        {
            throw new Exception("No se pudo actualizar el estado del pedido");
        }
    }

    public void Alta(Pedido NewPedido)
    {
        
        var parametetros = new DynamicParameters();
        parametetros.Add("xName",NewPedido.NombrePedido);
        parametetros.Add("xVolumen",NewPedido.Volumen);
        parametetros.Add("xPeso", NewPedido.Peso);
        parametetros.Add("xEstadoPedido", NewPedido.Estado);
        parametetros.Add("xFechaDespacho", NewPedido.FechaDespacho);
        parametetros.Add("xAdministrador_idAdministrador", NewPedido.XidAdministrador);
        parametetros.Add("xEmpresaDestino", NewPedido.XidEmpresa);
        parametetros.Add("xRuta_idRuta", NewPedido.XidRuta);

        try
        {
            Conexion.Execute("SPCrearPedido", parametetros);
        }
        catch (System.Exception)
        {
            throw new Exception("Error al intentar crear un pedido");
        }
    }

    public void AsignarVehiculo(int Pedidoid, int Vehiculoid)
    {
        var parametetros = new DynamicParameters();
        parametetros.Add("xidPedido", Pedidoid);
        parametetros.Add("xidVehiculo", Vehiculoid);
        try
        {
            Conexion.Execute("SPAsignarPedidoAVehiculo", parametetros);
        }
        catch (System.Exception)
        {
            throw new Exception("No se pudo asignar un vehiculo al pedido");
        }
    }

    public Pedido? Detalle(int xidPedido)
    {
        var Query=@"Select * from Pedido where idPedido = {xidPedido}";
        var repuesta = Conexion.QueryFirstOrDefault<Pedido>(Query);
        return repuesta;
    }

    public IEnumerable<Pedido> Obtener()
    {
        var Query=@"Select * from Pedido";
        var repuesta = Conexion.Query<Pedido>(Query);
        return repuesta;
    }

    public Pedido? ObtenerPedidoXCondicion(DateTime xtiempo)
    {
        var Query=@"Select * from Pedido where FechaDespacho = {xtiempo}";
        var repuesta = Conexion.QueryFirstOrDefault<Pedido>(Query);
        return repuesta;
    }
}
