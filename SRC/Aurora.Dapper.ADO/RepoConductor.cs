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

    Task<IEnumerable<Conductor>> IRepoListado<Conductor>.Obtener => throw new NotImplementedException();

    public async Task Alta(Conductor elemento)
    {
        var parametros = new DynamicParameters();
        parametros.Add("xNombre", elemento.Nombre);
        parametros.Add("xLicencia", elemento.Licencia);
        parametros.Add("xDisponibilidad", elemento.Dispobilidad);

        try
        {
            await Conexion.ExecuteAsync("SPNewConductor", parametros);
        }
        catch (System.Exception)
        {
            throw new Exception("Conductor ya registrado");
        }    
    }

    public async Task AsignarVehiculo(int conductorId, int vehiculoId)
    {
        var parametros = new DynamicParameters();
        parametros.Add("xidConductor", conductorId);
        parametros.Add("xidVehiculo", vehiculoId);

        try
        {
            await Conexion.ExecuteAsync("AsignarVehiculoAConductor", parametros);
        }
        catch (System.Exception)
        {
            throw new Exception("No se pudo Asignar el vehiculo al conductor");
        }    
    }

    public async Task DesasignarVehiculoDeConductor(int conductorId, int vehiculoId)
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

    public async Task<Conductor>? Detalle(int indiceABuscar)
    {
        var Query = @"Select * From Conductor Where idConductor = {indiceABuscar}";
        var resultados = await Conexion.QueryFirstOrDefaultAsync<Conductor>(Query);
        return resultados;
    }

    public async Task EliminarConductor(int idConductor)
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

    public async Task<IEnumerable<Conductor>> Obtener()
    {
        var Query = @"Select * From Conductor";
        var resultados = await Conexion.QueryAsync<Conductor>(Query);
        return resultados;
    }

    public async Task<bool> VefLicencia(string Licencia, int idConductor, int idVehiculo)
    {
        var resultado = await Conexion.ExecuteScalarAsync<bool>
        (
            "SELECT VerificarLicenciaValidaParaVehiculo(@idConductor, @idVehiculo)", new { idConductor, idVehiculo }
        );
        return resultado;
    }

    public async Task<Conductor> VerDisponibilidad(int conductorId)
    {
        var Query = @"Select Nombre, Licencia, Disponibilidad from Conductor where idConductor = {conductorId}";
        var resultados = await Conexion.QueryFirstOrDefaultAsync<Conductor>(Query);
        return resultados;
    }

    Task IRepoConductor.VefLicencia(string Licencia, int idVehiculo, int idConductor)
    {
        return VefLicencia(Licencia, idVehiculo, idConductor);
    }
}
