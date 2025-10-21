using System.Data;
using System.Runtime.CompilerServices;
using Aurora.Core;
using Aurora.Core.Interfaces;
using Aurora.Core.Models;
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

    public async Task<bool> ActualizarEstadoPedidoPorConductor(int idConductor, string nuevoEstado)
    {
        var parametros = new DynamicParameters();
        parametros.Add("xIdVehiculo", idConductor);
        parametros.Add("xNuevoEstado", nuevoEstado);

        try
        {
            var resultado = await Conexion.ExecuteAsync("SPActualizarEstadoPedidosPorVehiculo", parametros, commandType: CommandType.StoredProcedure);
            return resultado > 0;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al actualizar los pedidos del conductor: {ex.Message}");
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
            await ActualizarPesoVehiculo(_nuevoPedido.IdPedido, _nuevoPedido.xidVehiculo);
        }
        catch (System.Exception)
        {
            throw new Exception("Error al intentar crear un pedido");
        }
    }

    public async Task<bool> ActualizarPesoVehiculo(int idPedido, int idVehiculo)
    {
        var parametros = new DynamicParameters();
        parametros.Add("xidVehiculo", idVehiculo);
        parametros.Add("xidpedido", idPedido);

        try
        {
            var filasAfectadas = await Conexion.ExecuteAsync("SPRestarPesoVehiculo", parametros, commandType: CommandType.StoredProcedure);
            return filasAfectadas > 0;
        }
        catch (Exception)
        {
            throw new Exception("Error al actualizar el peso del vehículo");
        }
    }

    public async Task<Pedido>? DetalleAsync(int indiceABuscar)
    {
        var Query = @"Select * from Pedido where idPedido = @xidPedido";
        var repuesta = await Conexion.QueryFirstOrDefaultAsync<Pedido>(Query, new { xidPedido = indiceABuscar });
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

    public async Task<List<PedidoRutaDTO>> ObtenerPedidosEmpresa(int idEmpresa)
    {
        var query = @"SELECT 
                    p.idPedido,
                    p.Name AS NombrePedido,
                    p.Peso,
                    p.Volumen,
                    p.EstadoPedido,
                    p.FechaDespacho,
                    r.Origen,
                    r.Destino,
                    e.Nombre AS EmpresaOrigen
                FROM Pedido p
                INNER JOIN Ruta r 
                    ON p.idRuta = r.idRuta
                INNER JOIN Empresa e 
                    ON p.idEmpresa = e.idEmpresa
                WHERE p.EstadoPedido = 'Entregado'
                AND p.idEmpresa = @idEmpresa;
                ";

        return (List<PedidoRutaDTO>)await Conexion.QueryAsync<PedidoRutaDTO>(query, new { IdEmpresa = idEmpresa });
    }

    public async Task<List<PedidoRutaDTO>> ObtenerPedidosPorVehiculo(int idVehiculo)
    {
        var query = @"SELECT 
                    p.idPedido,
                    p.Name AS NombrePedido,
                    p.Peso,
                    p.Volumen,
                    p.EstadoPedido AS Estado,
                    p.FechaDespacho,
                    e.Nombre AS EmpresaOrigen,
                    r.Origen,
                    r.Destino
                FROM Pedido p
                INNER JOIN Empresa e ON p.idEmpresa = e.idEmpresa
                INNER JOIN Ruta r ON p.idRuta = r.idRuta
                WHERE p.idVehiculo = @idVehiculo and EstadoPedido != 'Entregado';
                ";
        return (List<PedidoRutaDTO>)await Conexion.QueryAsync<PedidoRutaDTO>(query, new { idVehiculo = idVehiculo });
    }
}
