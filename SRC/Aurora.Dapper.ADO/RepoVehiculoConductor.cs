using System.Data;
using Aurora.Core;
using Aurora.Core.Interfaces;
using Aurora.Core.Models;
using Dapper;

namespace Aurora.Dapper.ADO;

public class RepoVehiculoConductor : RepoGenerico, IRepoVehiculoConductor
{
    public RepoVehiculoConductor(IDbConnection conexion) : base(conexion) { }

    public Task<IEnumerable<VehiculoConductor>> ObtenerAsync => throw new NotImplementedException();

    public async Task AltaAsync(VehiculoConductor elemento)
    {
        var parametros = new DynamicParameters();
        parametros.Add("XidConductor", elemento.XidConductor);
        parametros.Add("XidVehiculo", elemento.XidVehiculo);
        parametros.Add("FechaAsignacion", elemento.FechaAsignacion); //este esta de mas (:)

        try
        {
            await Conexion.ExecuteAsync("AsignarVehiculoAConductor", parametros, commandType: CommandType.StoredProcedure);
        }
        catch (System.Exception e)
        {
            throw new Exception("Error al asignar vehiculo a conductor", e);
        }

    }

    public async Task<IEnumerable<VehiculoConductorDTO>> ConsultaConductoresAsignados()
    {
        var query = @"SELECT 
                        c.idConductor, c.Name AS NombreConductor, c.Licencia, c.Disponibilidad,
                        v.idVehiculo, v.Matricula, v.Tipo, v.CapacidadMax, v.Estado,
                        cv.FechaAsignado
                    FROM Conductor c
                    INNER JOIN Conductor_has_Vehiculo cv ON c.idConductor = cv.idConductor
                    INNER JOIN Vehiculo v ON cv.idVehiculo = v.idVehiculo;";
        return await Conexion.QueryAsync<VehiculoConductorDTO>(query);
    }

    public async Task<bool> DesasignarConductorDeVehiculo(int idVehiculo, int idConductor)
    {
        var parametros = new DynamicParameters();
        parametros.Add("xidConductor", idVehiculo);
        parametros.Add("xidVehiculo", idConductor);

        try
        {
            var resultado = await Conexion.ExecuteAsync("SPDesasignarVehiculoAConductor", parametros);
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
            throw new Exception("Error al querer cancelar la asignacion al conductor");
        }
    }

    public async Task AsignarConductorAVehiculo(int idVehiculo, int idConductor)
    {
        var parametros = new DynamicParameters();
        parametros.Add("xidConductor", idConductor);
        parametros.Add("xidVehiculo", idVehiculo);

        try
        {
            await Conexion.ExecuteAsync("AsignarVehiculoAConductor", parametros, commandType: CommandType.StoredProcedure);
        }
        catch (System.Exception)
        {
            throw new Exception("No se pudo Asignar el vehiculo al conductor");
        }    
    }

    public async Task<VehiculoConductor?> DetalleAsync(int idConductor)
    {
        var query = @"SELECT idVehiculo 
                    FROM Conductor_has_Vehiculo 
                    WHERE idConductor = @idConductor;";
        var resultado = await Conexion.QueryFirstOrDefaultAsync<VehiculoConductor>(query, new { idConductor });
        return resultado;
    }

    public async Task<int> ObtenerVehiculoPorIdConductor(int idConductor)
    {
        var query = @"SELECT idVehiculo 
                    FROM Conductor_has_Vehiculo 
                    WHERE idConductor = @idConductor;";
        var resultado = await Conexion.QueryFirstOrDefaultAsync<int>(query, new { idConductor });
        return resultado;
    }

    public async Task<IEnumerable<VehiculoConductorDTO>> consultaVehiculoLibres()
    {
        var query = @"SELECT 
                    v.idVehiculo, v.Matricula, v.Tipo, v.CapacidadMax
                FROM Vehiculo v
                WHERE v.idVehiculo NOT IN (
                    SELECT idVehiculo FROM Conductor_has_Vehiculo
                );";
        return await Conexion.QueryAsync<VehiculoConductorDTO>(query);
    }

    public async Task<IEnumerable<VehiculoConductorDTO>> consultaConductoresLibres()
    {
        var query = @"SELECT 
                    c.idConductor, c.Name AS NombreConductor, c.Licencia
                FROM Conductor c
                WHERE c.idConductor NOT IN (
                    SELECT idConductor FROM Conductor_has_Vehiculo
                );";
        return await Conexion.QueryAsync<VehiculoConductorDTO>(query);
    }

    public async Task<IEnumerable<VehiculoConductorDTO>> ConsultaConductoresNOAsignados()
    {
        var query = @"SELECT 
                            c.idConductor, c.Name AS NombreConductor, c.Licencia
                        FROM Conductor c
                        WHERE c.idConductor NOT IN (
                            SELECT idConductor FROM Conductor_has_Vehiculo
                        );";
        return await Conexion.QueryAsync<VehiculoConductorDTO>(query);    }
}
