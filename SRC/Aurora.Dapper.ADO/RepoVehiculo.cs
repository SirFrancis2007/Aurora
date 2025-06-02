using System.Data;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Schema;
using Aurora.Core;
using Aurora.Core.Interfaces;
using Dapper;

namespace Aurora.Dapper.ADO;

public class RepoVehiculo : RepoGenerico, IRepoVehiculo
{
    public RepoVehiculo(IDbConnection conexion) : base(conexion)
    {
    }

    public Task<IEnumerable<Vehiculo>> Obtener => throw new NotImplementedException();

    public async Task Alta(Vehiculo elemento)
    {
        var parametros = new DynamicParameters();
        parametros.Add("xTipo", elemento.Tipo);
        parametros.Add("xEstado", elemento.Estado);
        parametros.Add("xCapacidadMax", elemento.CapacidadMax);
        parametros.Add("xMatricula", elemento.Matricula);

        try
        {
            await Conexion.ExecuteAsync("SPCrearVehiculo", parametros);
        }
        catch (System.Exception)
        {
            throw new Exception(@"Error al agregar el vehiculo {Exception}");
        }    
    }

    public async Task<Boolean> CambiarEstado(int vehiculoId, bool disponible)
    {
        var parametros = new DynamicParameters();
        parametros.Add("xidVehiculo", vehiculoId);
        parametros.Add("xdisponible", disponible);    

        try
        {
            if (Convert.ToBoolean(Conexion.Execute("SPActualizarEstadoVehiculo", parametros)))
                return true;
            else
                return false;
        }
        catch (System.Exception)
        {
            throw new Exception("Error al actualizar el estado del vehiculo");
        }    }

    public async Task<Vehiculo>? Detalle(int indiceABuscar)
    {
        string query = @"
            SELECT *
            FROM Vehiculo 
            WHERE idVehiculo = @vehiculoId";

        var vehiculo = await Conexion.QueryFirstOrDefaultAsync<Vehiculo>(query, new { vehiculoId = indiceABuscar });
        return vehiculo;    
    }

    public async Task<Boolean> EliminarVehiculo(int idVehiculo)
    {
        var parametros = new DynamicParameters();
        parametros.Add("xidVehiculo", idVehiculo);
        try
        {
            if (Convert.ToBoolean(Conexion.Execute("SPDelVehiculo", parametros)))
                return true;
            else
                return false;
        }
        catch (System.Exception)
        {
            throw new Exception("Error al eliminar el vehiculo");
        }
    }

    public async Task<Pedido> ListarPedidosAsignados(int vehiculoId)
    {
        string query = @"
            SELECT p.idPedido, p.Name, p.Volumen, p.Peso, p.EstadoPedido, 
                   p.FechaDespacho, p.Administrador_idAdministrador, 
                   p.EmpresaDestino, p.Ruta_idRuta
            FROM Vehiculo_has_Pedido vhp
            INNER JOIN Pedido p ON vhp.Pedido_idPedido = p.idPedido
            WHERE vhp.Vehiculo_idVehiculo = @vehiculoId";

        var pedidos = await Conexion.QueryAsync<Pedido>(query, new { vehiculoId });
        return (Pedido)pedidos;
    }

    public async Task<IEnumerable<Vehiculo>> ObtenerData()
    {
        string query = @"SELECT * FROM Vehiculo";
        var pedidos = await Conexion.QueryAsync<Pedido>(query);
        return (IEnumerable<Vehiculo>)pedidos;
    }
}
