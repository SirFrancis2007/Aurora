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

    public Task<IEnumerable<Vehiculo>> ObtenerAsync => ObtenerData();

    public async Task<IEnumerable<Vehiculo>> ObtenerData() 
    {
        string query = @"SELECT * FROM Vehiculo";
        var repuesta = await Conexion.QueryAsync<Vehiculo>(query);
        return repuesta;
    }

    public async Task AltaAsync(Vehiculo elemento)
    {
        var parametros = new DynamicParameters();
        parametros.Add("xidVehiculo", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parametros.Add("xTipo", elemento.Tipo);
        parametros.Add("xEstado", elemento.Estado);
        parametros.Add("xCapacidadMax", elemento.CapacidadMax);
        parametros.Add("xMatricula", elemento.Matricula);

        try
        {
            await Conexion.ExecuteAsync("SPCrearVehiculo", parametros,commandType: CommandType.StoredProcedure);

            var nuevo_idvehiculo = parametros.Get<int>("xidVehiculo");
            elemento.IdVehiculo = nuevo_idvehiculo;
        }
        catch (System.Exception)
        {
            throw new Exception(@"Error al agregar el vehiculo");
        }    
    }

    public async Task<Boolean> CambiarEstadoAsync(int vehiculoId, bool disponible)
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

    public async Task<Vehiculo>? DetalleAsync(int indiceABuscar)
    {
        var query = @"
            SELECT *
            FROM Vehiculo 
            WHERE idVehiculo = @vehiculoId";

        var vehiculo = await Conexion.QueryFirstOrDefaultAsync<Vehiculo>(query, new { vehiculoId = indiceABuscar });
        return vehiculo;    
    }

    public async Task<Boolean> EliminarVehiculoAsync(int idVehiculo)
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
}
