using System.Data;
using System.Linq.Expressions;
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
    public void Alta(Vehiculo elemento)
    {
        var parametros = new DynamicParameters();
        parametros.Add("xTipo", elemento.Tipo);
        parametros.Add("xEstado", elemento.Estado);
        parametros.Add("xCapacidadMax", elemento.CapacidadMax);
        parametros.Add("xMatricula", elemento.Matricula);

        try
        {
            Conexion.Execute("SPCrearVehiculo", parametros);
        }
        catch (System.Exception)
        {
            throw new Exception(@"Error al agregar el vehiculo {Exception}");
        }
    }

    public bool EliminarVehiculo(int idVehiculo)
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

    public bool CambiarEstado(int vehiculoId, bool disponible)
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
        }
    }

    public List<Pedido> ListarPedidosAsignados(int vehiculoId)
    {
        string query = @"
            SELECT p.idPedido, p.Name, p.Volumen, p.Peso, p.EstadoPedido, 
                   p.FechaDespacho, p.Administrador_idAdministrador, 
                   p.EmpresaDestino, p.Ruta_idRuta
            FROM Vehiculo vhp
            INNER JOIN Pedido p ON vhp.Pedido_idPedido = p.idPedido
            WHERE vhp.Vehiculo_idVehiculo = @vehiculoId";

        var pedidos = Conexion.Query<Pedido>(query, new { vehiculoId }).ToList();
        return pedidos;
    }

    Vehiculo? IRepoDetalle<Vehiculo, int>.Detalle(int indiceABuscar)
    {
        string query = @"
            SELECT *
            FROM Vehiculo 
            WHERE idVehiculo = @vehiculoId";

        var vehiculo = Conexion.QueryFirstOrDefault<Vehiculo>(query, new { vehiculoId = indiceABuscar });
        return vehiculo;    
    }

    IEnumerable<Vehiculo> IRepoListado<Vehiculo>.Obtener()
    {
        string query = @"
            SELECT *
            FROM Vehiculo";

        var pedidos = Conexion.Query<Pedido>(query).ToList();
        return (IEnumerable<Vehiculo>)pedidos;
    }
}
