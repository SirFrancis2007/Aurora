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

    public async Task AltaAsync(Administrador NewAdmin)
    {
        var parametros = new DynamicParameters();
        parametros.Add("xidAdministrador", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parametros.Add("xName", NewAdmin.Nombre);
        parametros.Add("xPassword", NewAdmin.Password);
        parametros.Add("xidEmpresa", NewAdmin.IdEmpresa);

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

    public async Task<IEnumerable<Administrador>> ObtenerData()
    {
        var Query = @"SELECT Nombre, idEmpresa, Contrasena FROM Administrador";
        var repuesta = await Conexion.QueryAsync<Administrador>(Query);
        return repuesta;
    }

    public async Task<IEnumerable<Administrador>> ObtenerDataXidAsync(int ID)
    {
        var Query = @"SELECT * FROM Administrador where idAdministrador = @xidAdmin;";
        var repuesta = await Conexion.QueryAsync<Administrador>(Query, new {xidAdmin = ID});
        return repuesta;
    }


    async Task<bool> IRepoAdministrador.UpdateAdministrador(Administrador administrador)
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
