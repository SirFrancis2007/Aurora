using System.Data;
using System.Runtime.CompilerServices;
using Aurora.Core;
using Aurora.Core.Interfaces;
using Dapper;

namespace Aurora.Dapper.ADO;

public class RepoPedido : RepoGenerico, IRepoPedido
{
    public RepoPedido(IDbConnection conexion) : base(conexion) {}

    Task<IEnumerable<Pedido>> IRepoListado<Pedido>.Obtener => throw new NotImplementedException();

    public async Task Alta(Pedido NewPedido)
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
            await Conexion.ExecuteAsync("SPCrearPedido", parametetros);
        }
        catch (System.Exception)
        {
            throw new Exception("Error al intentar crear un pedido");
        }    
    }
    
    public async Task<IEnumerable<Pedido>> ObtenerData()
    {
        var Query=@"Select * from Pedido";
        var repuesta = await Conexion.QueryAsync<Pedido>(Query);
        return repuesta;
    }

    public async Task ActualizarEstado(int pedidoId, string nuevoEstado)
    {
        var parametetros = new DynamicParameters();
        parametetros.Add("xidPedido",pedidoId);
        parametetros.Add("xNuevoEstado",nuevoEstado);
        try
        {
            await Conexion.ExecuteAsync("SPUpdateEstadoPedido", parametetros);
        }
        catch (System.Exception)
        {
            throw new Exception("No se pudo actualizar el estado del pedido");
        }   
    }

    public async Task AsignarVehiculo(int pedidoId, int vehiculoId)
    {
        var parametetros = new DynamicParameters();
        parametetros.Add("xidPedido", pedidoId);
        parametetros.Add("xidVehiculo", vehiculoId);
        try
        {
            await Conexion.ExecuteAsync("SPAsignarPedidoAVehiculo", parametetros);
        }
        catch (System.Exception)
        {
            throw new Exception("No se pudo asignar un vehiculo al pedido");
        }    
    }

    public async Task<Pedido>? Detalle(int indiceABuscar)
    {
        var Query=@"Select * from Pedido where idPedido = {xidPedido}";
        var repuesta = await Conexion.QueryFirstOrDefaultAsync<Pedido>(Query);
        return repuesta;    
    }

    public async Task<Pedido>? ObtenerPedidoXCondicion(DateTime Xfecha)
    {
        var Query=@"Select * from Pedido where FechaDespacho = {xtiempo}";
        var repuesta = await Conexion.QueryFirstOrDefaultAsync<Pedido>(Query);
        return repuesta;
    }
}
