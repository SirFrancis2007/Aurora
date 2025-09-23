using System.Data;
using System.Runtime.CompilerServices;
using Aurora.Core;
using Aurora.Core.Interfaces;
using Aurora.Core.Models;
using Dapper;

namespace Aurora.Dapper.ADO;

public class RepoVehiculoConductor : RepoGenerico, IRepoVehiculoConductor
{
    public RepoVehiculoConductor(IDbConnection conexion) : base(conexion) { }

    Task<IEnumerable<VehiculoConductor>> IRepoListado<VehiculoConductor>.ObtenerAsync => throw new NotImplementedException();

    public async Task AltaAsync(VehiculoConductor elemento)
    {
        var parametros = new DynamicParameters();
        parametros.Add("XidConductor", elemento.XidConductor);
        parametros.Add("XidVehiculo", elemento.XidVehiculo);
        parametros.Add("FechaAsignacion", elemento.FechaAsignacion);

        try
        {
            await Conexion.ExecuteAsync("AsignarVehiculoAConductor", parametros, commandType: CommandType.StoredProcedure);
        }
        catch (System.Exception e)
        {
            throw new Exception("Error al asignar vehiculo a conductor", e);
        }
    }

    public async Task<VehiculoConductor>? DetalleAsync(int indiceABuscar)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<VehiculoConductorDTO>> Consulta()
    {
        var query = @"SELECT 
    c.idConductor, c.Name AS NombreConductor, c.Licencia, c.Disponibilidad,
    v.idVehiculo, v.Matricula, v.Tipo, v.CapacidadMax, v.Estado AS EstadoVehiculo,
    cv.FechaAsignado
FROM Conductor c
LEFT JOIN Conductor_has_Vehiculo cv ON c.idConductor = cv.idConductor
LEFT JOIN Vehiculo v ON cv.idVehiculo = v.idVehiculo

UNION

SELECT 
    NULL AS idConductor, NULL AS NombreConductor, NULL AS Licencia, NULL AS Disponibilidad,
    v.idVehiculo, v.Matricula, v.Tipo, v.CapacidadMax, v.Estado AS EstadoVehiculo,
    NULL AS FechaAsignado
FROM Vehiculo v
WHERE v.idVehiculo NOT IN (SELECT idVehiculo FROM Conductor_has_Vehiculo);";

        return await Conexion.QueryAsync<VehiculoConductorDTO>(query);
    }

}
