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

    public async Task<Conductor?> Loguearse(string nombre, string contrasena)
    {
        var funtionLogin = "SELECT FLoginConductor(@xNombre, @xLicencia);"; // Crear en bd la funcion FLoginConductor
        var result = await Conexion.ExecuteScalarAsync<Conductor>(funtionLogin, new { xNombre = nombre, xLicencia = contrasena });
        return result;
    }
    
    public async Task<IEnumerable<Conductor>> ObtenerDataAsync()
    {
        var Query = @"Select * From Conductor";
        var resultados = await Conexion.QueryAsync<Conductor>(Query);
        return resultados;
    }

}
