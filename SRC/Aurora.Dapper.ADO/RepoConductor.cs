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

    public void Alta(Conductor elemento)
    {
        var parametros = new DynamicParameters();
        parametros.Add("xNombre", elemento.Nombre);
        parametros.Add("xLicencia", elemento.Licencia);
        parametros.Add("xDisponibilidad", elemento.Dispobilidad);

        try
        {
            Conexion.Execute("SPNewConductor", parametros);
        }
        catch (System.Exception)
        {
            throw new Exception("Conductor ya registrado");
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

    public void AsignarVehiculo(int conductorId, int vehiculoId)
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

    public void DesasignarVehiculoDeConductor(int conductorId, int vehiculoId)
    {
        var parametros = new DynamicParameters();
        parametros.Add("xidConductor", conductorId);
        parametros.Add("xidVehiculo", vehiculoId);

        try
        {
            Conexion.Execute("SPDesasignarVehiculoAConductor", parametros);
        }
        catch (System.Exception)
        {
            throw new Exception("Error al querer cancelar la asignacion al conductor");
        }    
    }

    public Conductor? Detalle(int indiceABuscar)
    {
        var Query = @"Select * From Conductor Where idConductor = {indiceABuscar}";
        var resultados = Conexion.QueryFirstOrDefault<Conductor>(Query);
        return resultados;
    }

    public IEnumerable<Conductor> Obtener()
    {
        var Query = @"Select * From Conductor";
        var resultados = Conexion.Query<Conductor>(Query);
        return resultados;
    }

    public bool VefLicencia(string Licencia, int idConductor, int idVehiculo)
    {
        var resultado = Conexion.ExecuteScalar<bool>
        (
            "SELECT VerificarLicenciaValidaParaVehiculo(@idConductor, @idVehiculo)", new { idConductor, idVehiculo }
        );
        return resultado;
    }

    public Conductor VerDisponibilidad(int conductorId)
    {
        var Query = @"Select Nombre, Licencia, Disponibilidad from Conductor where idConductor = {conductorId}";
        var resultados = Conexion.QueryFirstOrDefault<Conductor>(Query);
        return resultados;
    }
}
