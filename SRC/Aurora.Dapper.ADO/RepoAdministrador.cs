using System.Data;
using Aurora.Core;
using Aurora.Core.Interfaces;
using Dapper;
#pragma warning restore format
namespace Aurora.Dapper.ADO;

public class RepoAdministrador : RepoGenerico, IRepoAdministrador
{
    public RepoAdministrador(IDbConnection conexion) : base(conexion)
    {
    }

    public Task<IEnumerable<Administrador>> ObtenerAsync => ObtenerData();

    public async Task AltaAsync(Administrador _nuevoadministrador)
    {
        var parametros = new DynamicParameters();
        parametros.Add("xidAdministrador", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parametros.Add("xName", _nuevoadministrador.Nombre);
        parametros.Add("xPassword", _nuevoadministrador.Password);
        parametros.Add("xidEmpresa", _nuevoadministrador.IdEmpresa);

        try
        {
            await Conexion.ExecuteAsync("SPNuevoAdministrador", parametros, commandType: CommandType.StoredProcedure);
        }
        catch (System.Exception)
        {
            throw new Exception("Error al agregar un nuevo administrador");
        }
    }

    public async Task<Administrador>? DetalleAsync(int xidAdmin)
    {
        var Query = @"SELECT idAdministrador, Nombre, idEmpresa, Contrasena FROM Administrador where idAdministrador = @xidAdmin";
        var repuesta = await Conexion.QueryFirstOrDefaultAsync<Administrador>(Query, new { xidAdmin });
        return repuesta;
    }

    public async Task<bool> EliminarAsync(int _idadministrador)
    {
        var parametros = new DynamicParameters();
        parametros.Add("xidAdministrador", _idadministrador);
        try
        {
            await Conexion.ExecuteAsync("SPDelAdministrador", parametros);
            return true;
        }
        catch (System.Exception)
        {
            throw new Exception("¡Error al eliminar al administrador!");
        }
    }

    public async Task<bool> LoguearseAsync(string nombre, string contrasena)
    {
        var funtionLogin = "SELECT FLoginAdministrador(@xNombre, @xPassword);";
        var result = await Conexion.ExecuteScalarAsync<bool>(funtionLogin, new { xNombre = nombre, xPassword = contrasena });
        return result;
    }

    public async Task<IEnumerable<Administrador>> ObtenerData()
    {
        var Query = @"SELECT Nombre, idEmpresa, Contrasena FROM Administrador";
        var repuesta = await Conexion.QueryAsync<Administrador>(Query);
        return repuesta;
    }

    public async Task<List<Administrador>> ObtenerPorEmpresaAsync(int idEmpresa)
    {
        var query = @"Select * From Administrador where idEmpresa = @IdEmpresa;";
        var Resultado = await Conexion.QueryAsync<Administrador>(query, new { IdEmpresa = idEmpresa });
        return (List<Administrador>)Resultado;
    }

    public async Task<bool> UpdateAdministrador(Administrador administrador)
    {
        var parametros = new DynamicParameters();
        parametros.Add("xidAdministrador", administrador.IdAdministrador);
        parametros.Add("xName", administrador.Nombre);
        parametros.Add("xPassword", administrador.Password);
        parametros.Add("xidEmpresa", administrador.IdEmpresa);

        try
        {
            await Conexion.ExecuteAsync("SPUpdateAdmi", parametros, commandType: CommandType.StoredProcedure);
        }
        catch (System.Exception e)
        {
            throw new Exception("¡Error al actualizar el administrador!", e);
        }
        return true;
    }
}
