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

    public Task<IEnumerable<Administrador>> Obtener => throw new NotImplementedException();

    public async Task Alta(Administrador NewAdmin)
    {
        var parametros = new DynamicParameters();
        parametros.Add("xName",NewAdmin.Nombre);
        parametros.Add("xPassword", NewAdmin.Password);
        parametros.Add("xEmpresa_idEmpresa", NewAdmin.IdEmpresa);

        try
        {
            await Conexion.ExecuteAsync("SPNuevoAdministrador", parametros, commandType: CommandType.StoredProcedure);
        }
        catch (System.Exception)
        {
            throw new Exception("Error al agregar un nuevo administrador");
        }    
    }

    public async Task<Administrador>? Detalle(int xidAdmin)
    {
        var Query = @"SELECT * FROM Administrador where idAdministrador = {xidAdmin}";
        var repuesta = await Conexion.QueryFirstOrDefaultAsync<Administrador>(Query, new { xidAdmin });
        return repuesta;        
    }

    public async Task<Pedido> ObtenerPedidoXAdmin(int idadministrador)
    {
        var Query = @"SELECT * FROM Administrador";
        var repuesta = await Conexion.QueryAsync<Administrador>(Query);
        return (Pedido)repuesta;    
    }
}
