using System.Data;
using System.Text;
using Aurora.Core;
using Aurora.Core.Interfaces;
using Dapper;

namespace Aurora.Dapper.ADO;

public class RepoConductor : RepoGenerico, IRepoConductor
{
    public RepoConductor(IDbConnection conexion) : base(conexion)
    {
    }

    Task<IEnumerable<Conductor>> IRepoListado<Conductor>.ObtenerAsync => ObtenerDataAsync();

    public async Task AltaAsync(Conductor elemento)
    {
        var parametros = new DynamicParameters();
        parametros.Add("xidConductor", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parametros.Add("xName", elemento.Name);
        parametros.Add("xLicencia", elemento.Licencia);
        parametros.Add("xDisponibilidad", elemento.Dispobilidad);

        try
        {
            await Conexion.ExecuteAsync("SPNewConductor", parametros, commandType: CommandType.StoredProcedure);
            elemento.IdConductor = parametros.Get<int>("xidConductor");
        }
        catch (System.Exception)
        {
            throw new Exception("Conductor ya registrado");
        }    
    }

    public async Task AsignarVehiculoAsync(int conductorId, int vehiculoId)
    {
        var parametros = new DynamicParameters();
        parametros.Add("xidConductor", conductorId);
        parametros.Add("xidVehiculo", vehiculoId);

        try
        {
            await Conexion.ExecuteAsync("AsignarVehiculoAConductor", parametros, commandType: CommandType.StoredProcedure);
        }
        catch (System.Exception)
        {
            throw new Exception("No se pudo Asignar el vehiculo al conductor");
        }    
    }

    public async Task DesasignarVehiculoDeConductorAsync(int conductorId, int vehiculoId)
    {
        var parametros = new DynamicParameters();
        parametros.Add("xidConductor", conductorId);
        parametros.Add("xidVehiculo", vehiculoId);

        try
        {
            await Conexion.ExecuteAsync("SPDesasignarVehiculoAConductor", parametros);
        }
        catch (System.Exception)
        {
            throw new Exception("Error al querer cancelar la asignacion al conductor");
        }    
    }

    public async Task<Conductor>? DetalleAsync(int xidConductor)
    {
        var Query = @"Select idConductor, Name, Licencia, Disponibilidad From Conductor Where idConductor = @indice;";
        var resultados = await Conexion.QueryFirstOrDefaultAsync<Conductor>(Query, new {indice = xidConductor});
        return resultados;
    }

    public async Task EliminarConductorAsync(int idConductor)
    {
        var parametros = new DynamicParameters();
        parametros.Add("xidConductor", idConductor);

        try
        {
            await Conexion.ExecuteAsync("SPDelConductor", parametros);
        }
        catch (System.Exception)
        {
            throw new Exception("Error al eliminar al conductor");
        }    
    }

    public async Task<IEnumerable<Conductor>> ObtenerDataAsync()
    {
        var Query = @"Select * From Conductor";
        var resultados = await Conexion.QueryAsync<Conductor>(Query);
        return resultados;
    }

    public async Task<bool> VefLicenciaAsync(string Licencia, int idConductor, int idVehiculo)
    {
        var resultado = await Conexion.ExecuteScalarAsync<bool>
        (
            "SELECT VerificarLicenciaValidaParaVehiculo(@idConductor, @idVehiculo)", new { idConductor, idVehiculo }
        );
        return resultado;
    }

    public async Task<Conductor> VerDisponibilidadAsync(int conductorId)
    {
        var Query = @"Select Name, Licencia, Disponibilidad from Conductor where idConductor = @conductorId";
        var resultados = await Conexion.QueryFirstOrDefaultAsync<Conductor>(Query, new { conductorId });
        return resultados;
    }
}
