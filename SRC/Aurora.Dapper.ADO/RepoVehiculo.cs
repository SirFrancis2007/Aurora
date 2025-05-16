using System.Data;
using System.Linq.Expressions;
using Aurora.Core;
using Aurora.Core.Interfaces;
using Dapper;

namespace Aurora.Dapper.ADO;

public class RepoVehiculo : RepoGenerico, IRepoVehiculo
{
    public RepoVehiculo(IDbConnection conexion) : base(conexion)
    {
    }

    public void AñadirVehiculo(Vehiculo NewVehiculo)
    {
        var parametros = new DynamicParameters();
        parametros.Add("xTipo", NewVehiculo.Tipo);
        parametros.Add("xEstado", NewVehiculo.Estado);
        parametros.Add("xCapacidadMax", NewVehiculo.CapacidadMax);
        parametros.Add("xMatricula", NewVehiculo.Matricula);

        try
        {
            Conexion.Execute("SPCrearVehiculo", parametros);
        }
        catch (System.Exception)
        {
            throw new Exception(@"Error al agregar el vehiculo {Exception}");
        }
    }

    public void CambiarEstado(int vehiculoId, bool disponible)
    {
        throw new NotImplementedException();
    }

    public void EliminarVehiculo(int idVehiculo)
    {
        var parametros = new DynamicParameters();
        parametros.Add("xidVehiculo", idVehiculo);
        try
        {
            Conexion.Execute("SPDelVehiculo", parametros);
        }
        catch (System.Exception)
        {
            throw new Exception("Error al eliminar el vehiculo");
        }
    }

    public List<Pedido> ListarPedidosAsignados(int vehiculoId)
    {
        string query = @"
            SELECT p.idPedido, p.Name, p.Volumen, p.Peso, p.EstadoPedido, 
                   p.FechaDespacho, p.Administrador_idAdministrador, 
                   p.EmpresaDestino, p.Ruta_idRuta
            FROM Vehiculo_has_Pedido vhp
            INNER JOIN Pedido p ON vhp.Pedido_idPedido = p.idPedido
            WHERE vhp.Vehiculo_idVehiculo = @vehiculoId";

        var pedidos = Conexion.Query<Pedido>(query, new { vehiculoId }).ToList();
        return pedidos;
    }
}
