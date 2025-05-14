using System.Data;
using Aurora.Core;
using Aurora.Core.Interfaces;
using Dapper;

namespace Aurora.Dapper.ADO;

public class RepoConductor : RepoGenerico, IRepoConductor
{
    public RepoConductor(IDbConnection conexion) : base(conexion)
    {
    }

    public void AsignarVehiculo(int conductorId, int vehiculoId, DateTime fechaAsignacion)
    {
        var parametros = new DynamicParameters();
        parametros.Add("xidConductor", conductorId);
        parametros.Add("xidVehiculo", vehiculoId);

        try
        {
            Conexion.Execute("AsignarVehiculoAConductor", parametros);
        }
        catch (System.Exception)
        {
            throw new Exception("No se pudo Asignar el vehiculo al conductor");
        }
    }

    public void CrearConductor(Conductor NewConductor)
    {
        var parametros = new DynamicParameters();
        parametros.Add("xNombre", NewConductor.Nombre);
        parametros.Add("xLicencia", NewConductor.Licencia);
        parametros.Add("xDisponibilidad", NewConductor.Dispobilidad);

        try
        {
            Conexion.Execute("SPNewConductor", parametros);
        }
        catch (System.Exception)
        {
            throw new Exception("Conductor ya registrado");
        }
    }

    public void DesasignarVehiculoDeConductor(int conductorId, int vehiculoId)
    {
        var parametros = new DynamicParameters();
        parametros.Add("xidConductor", conductorId);
        parametros.Add("xidVehiculo", vehiculoId);

        try
        {
            Conexion.Execute("DesasignarVehiculoDeConductor", parametros);
        }
        catch (System.Exception)
        {
            throw new Exception("No se pudo Asignar el vehiculo al conductor");
        }
    }


    public void EliminarConductor(int idConductor)
    {
        var parametros = new DynamicParameters();
        parametros.Add("xidConductor", idConductor);
        
        try
        {
            Conexion.Execute("SPDelConductor", parametros);
        }
        catch (System.Exception)
        {
            throw new Exception("Error al eliminar al conductor");
        }
    }


    public Conductor VerDisponibilidad(int conductorId)
    {
        var Query = @"Select * from Conductor where idConductor = {conductorId}";
        var resultados = Conexion.QueryFirstOrDefault<Conductor>(Query);
        return resultados;
    }
    public bool VefLicencia(string Licencia)
    {
        throw new NotImplementedException();
    }
    public void AsignarVehiculo(int conductorId, int vehiculoId)
    {
        throw new NotImplementedException();
    }
}
