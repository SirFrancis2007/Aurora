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
        parametros.Add("xDisponibilidad", elemento.Disponibilidad);

        try
        {
            await Conexion.ExecuteAsync("SPNuevoConductor", parametros, commandType: CommandType.StoredProcedure);
            elemento.IdConductor = parametros.Get<int>("xidConductor");
        }
        catch (System.Exception e)
        {
            throw new Exception("Conductor ya registrado", e);
        }

    }

    public async Task<Conductor>? DetalleAsync(int xidConductor)
    {
        var Query = @"Select idConductor, Name, Licencia, Disponibilidad From Conductor Where idConductor = @indice;";
        var resultados = await Conexion.QueryFirstOrDefaultAsync<Conductor>(Query, new { indice = xidConductor });
        return resultados;

    }

    public async Task<bool> EliminarConductor(int idConductor)
    {
        var parametros = new DynamicParameters();
        parametros.Add("xidConductor", idConductor);

        try
        {
            var resultado = await Conexion.ExecuteAsync("SPDelConductor", parametros);
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
            throw new Exception("Error al eliminar al conductor");
        }

    }

    public async Task<List<Conductor>> ListarConductoresLibres()
    {
        var query = @"SELECT * FROM conductor WHERE Disponibilidad = 1;";
        var conductores = await Conexion.QueryAsync<Conductor>(query);
        return conductores.AsList();
    }

    public async Task<bool> Loguearse(string nombre, string contrasena)
    {
        var funtionLogin = "SELECT FLoginConductor(@xNombre, @xLicencia);"; 
        var result = await Conexion.ExecuteScalarAsync<bool>(funtionLogin, new { xNombre = nombre, xLicencia = contrasena });
        return result;
    }
    
    public async Task<IEnumerable<Conductor>> ObtenerDataAsync()
    {
        var Query = @"Select * From Conductor";
        var resultados = await Conexion.QueryAsync<Conductor>(Query);
        return resultados;
    }

    public async Task<bool> ActualizarConductor(Conductor _conductor)
    {
        var parametros = new DynamicParameters();
        parametros.Add("xidConductor", _conductor.IdConductor);
        parametros.Add("xnombre", _conductor.Name);
        parametros.Add("xLicencia", _conductor.Licencia);
        parametros.Add("xDisponibilidad", _conductor.Disponibilidad == true);

        try
        {
            await Conexion.ExecuteAsync("SPUpdateConductor", parametros, commandType: CommandType.StoredProcedure);
            return true;
        }
        catch (System.Exception e)
        {
            throw new Exception("¡Error al actualizar el administrador!", e);
        }
    }

    public async Task<Conductor> ObtenerConductorPorNombre(string nombre)
    {
        var query = @"SELECT idConductor, Name FROM Conductor WHERE Name = @nombre";
        var resultado = await Conexion.QueryFirstOrDefaultAsync<Conductor>(query, new { nombre });
        return resultado;
    }


    public async Task<bool> ActualizarEstadoConductor(int id)
    {
        // Ya en la Funcion se cambia el estado a en "En viaje"
        var funtionLogin = "SELECT FncActualizarEstadoConductor(@xidconductor);"; 
        var result = await Conexion.ExecuteScalarAsync<bool>(funtionLogin, new { xidconductor = id });
        return result;
    }

    public async Task<bool> FncLiberarEstadoConductor(int id)
    {
        var funtionLogin = "SELECT FncLiberarEstadoConductor(@xidconductor);"; 
        var result = await Conexion.ExecuteScalarAsync<bool>(funtionLogin, new { xidconductor = id });
        return result;
    }
}
