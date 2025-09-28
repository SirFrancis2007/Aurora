using System.Data;
using System.Runtime.CompilerServices;
using Aurora.Core;
using Aurora.Core.Interfaces;
using Dapper;

namespace Aurora.Dapper.ADO;

public class RepoPedido : RepoGenerico, IRepoPedido
{
    public RepoPedido(IDbConnection conexion) : base(conexion) {}
    Task<IEnumerable<Pedido>> IRepoListado<Pedido>.ObtenerAsync => ObtenerDataAsync(); 
    public async Task<IEnumerable<Pedido>> ObtenerDataAsync()
    {
        var Query=@"Select * from Pedido";
        var repuesta = await Conexion.QueryAsync<Pedido>(Query);
        return repuesta;
    }

    public async Task<bool> ActualizarEstadoPedidoPorAdmin(int idPedido, string nuevoEstado)
    {
        var parametetros = new DynamicParameters();
        parametetros.Add("xidPedido",idPedido);
        parametetros.Add("xNuevoEstado",nuevoEstado);
        try
        {
            var resultado = await Conexion.ExecuteAsync("SPUpdateEstadoPedido", parametetros, commandType: CommandType.StoredProcedure);
            if (resultado == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (System.Exception)
        {
            throw new Exception("No se pudo actualizar el estado del pedido");
        }
    }

    public async Task<bool> ActualizarEstadoPedidoPorConductor(int idPedido, string nuevoEstado)
    {
        var parametetros = new DynamicParameters();
        parametetros.Add("xidPedido",idPedido);
        parametetros.Add("xNuevoEstado",nuevoEstado);
        try
        {
            var resultado = await Conexion.ExecuteAsync("SPUpdateEstadoPedido", parametetros, commandType: CommandType.StoredProcedure);
            if (resultado == 1) return true;
            else { return false; }
        }
        catch (System.Exception)
        {
            throw new Exception("No se pudo actualizar el estado del pedido");
        }
    }

    public async Task AltaAsync(Pedido _nuevoPedido)
    {
        var parametros = new DynamicParameters();
        parametros.Add("xidPedido", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parametros.Add("xName",_nuevoPedido.NombrePedido);
        parametros.Add("xVolumen",_nuevoPedido.Volumen);
        parametros.Add("xPeso", _nuevoPedido.Peso);
        parametros.Add("xEstadoPedido", _nuevoPedido.Estado);
        parametros.Add("xFechaDespacho", _nuevoPedido.FechaDespacho);
        parametros.Add("xAdministrador_idAdministrador", _nuevoPedido.XidAdministrador);
        parametros.Add("xEmpresaDestino", _nuevoPedido.XidEmpresa);
        parametros.Add("xRuta_idRuta", _nuevoPedido.XidRuta);
        parametros.Add("xidVehiculo", _nuevoPedido.xidVehiculo);

        try
        {
            await Conexion.ExecuteAsync("SPCrearPedido", parametros, commandType: CommandType.StoredProcedure);
            _nuevoPedido.IdPedido = parametros.Get<int>("xidPedido");
        }
        catch (System.Exception)
        {
            throw new Exception("Error al intentar crear un pedido");
        }
    }

    public async Task<Pedido>? DetalleAsync(int indiceABuscar)
    {
        var Query=@"Select * from Pedido where idPedido = @xidPedido";
        var repuesta = await Conexion.QueryFirstOrDefaultAsync<Pedido>(Query, new {xidPedido = indiceABuscar});
        return repuesta;
    }

    public async Task<List<PedidoEmpresaDTO>> ObtenerPedidosPorEmpresa(int idEmpresa)
    {
        var query = @"
        SELECT 
            pe.idPedido AS IdPedido,
            pe.Name AS NombrePedido,
            pe.Volumen, 
            pe.Peso, 
            pe.EstadoPedido AS Estado,
            pe.FechaDespacho,
            emd.idEmpresa AS IdEmpresa,
            emd.Nombre AS NombreEmpresa
        FROM Empresa em
        JOIN Administrador admi USING (idEmpresa)
        JOIN Pedido pe ON pe.idAdministrador = admi.idAdministrador
        JOIN Empresa emd ON emd.idEmpresa = pe.idEmpresa
        WHERE em.idEmpresa = @IdEmpresa OR pe.idEmpresa = @IdEmpresa;
        ";

        return (List<PedidoEmpresaDTO>)await Conexion.QueryAsync<PedidoEmpresaDTO>(query, new { IdEmpresa = idEmpresa });
    }
}
