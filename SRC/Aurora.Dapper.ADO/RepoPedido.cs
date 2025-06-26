using System.Data;
using System.Runtime.CompilerServices;
using Aurora.Core;
using Aurora.Core.Interfaces;
using Dapper;

namespace Aurora.Dapper.ADO;

public class RepoPedido : RepoGenerico, IRepoPedido
{
    public RepoPedido(IDbConnection conexion) : base(conexion) {}

    Task<IEnumerable<Pedido>> IRepoListado<Pedido>.Obtener => ObtenerData();

    public async Task Alta(Pedido NewPedido)
    {
        var parametros = new DynamicParameters();
        parametros.Add("xidPedido", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parametros.Add("xName",NewPedido.NombrePedido);
        parametros.Add("xVolumen",NewPedido.Volumen);
        parametros.Add("xPeso", NewPedido.Peso);
        parametros.Add("xEstadoPedido", NewPedido.Estado);
        parametros.Add("xFechaDespacho", NewPedido.FechaDespacho);
        parametros.Add("xAdministrador_idAdministrador", NewPedido.XidAdministrador);
        parametros.Add("xEmpresaDestino", NewPedido.XidEmpresa);
        parametros.Add("xRuta_idRuta", NewPedido.XidRuta);
        parametros.Add("xidVehiculo", NewPedido.xidVehiculo);

        try
        {
            await Conexion.ExecuteAsync("SPCrearPedido", parametros, commandType: CommandType.StoredProcedure);
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
            await Conexion.ExecuteAsync("SPUpdateEstadoPedido", parametetros, commandType :CommandType.StoredProcedure);
        }
        catch (System.Exception)
        {
            throw new Exception("No se pudo actualizar el estado del pedido");
        }   
    }

    public async Task<Pedido>? Detalle(int indiceABuscar)
    {
        var Query=@"Select * from Pedido where idPedido = @xidPedido";
        var repuesta = await Conexion.QueryFirstOrDefaultAsync<Pedido>(Query, new {xidPedido = indiceABuscar});
        return repuesta;    
    }

    public async Task<Pedido>? ObtenerPedidoXCondicion(DateTime xfecha)
    {
        var Query=@"Select * from Pedido where FechaDespacho = @xtiempo";
        var repuesta = await Conexion.QueryFirstOrDefaultAsync<Pedido>(Query, new {xtiempo = xfecha});
        return repuesta;
    }
}
