using System.Data;
using Aurora.Core;
using Aurora.Core.Interfaces;
using Dapper;

namespace Aurora.Dapper.ADO;

public class RepoEmpresa : RepoGenerico, IRepoEmpresa
{
    Task<IEnumerable<Empresa>> IRepoListado<Empresa>.Obtener => Obtener();

    public RepoEmpresa(IDbConnection conexion) : base(conexion)
    {
    }

    public async Task<IEnumerable<Empresa>> Obtener()
    {
        var query = @"Select * From Empresa";
        var Resultado = await Conexion.QueryAsync<Empresa>(query);
        return Resultado;
    }

    public async Task Alta(Empresa NuevaEmpresa)
    {
        var parametros = new DynamicParameters();
        parametros.Add("xidEmpresa", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parametros.Add("xNombre", NuevaEmpresa.Nombre);
        try
        {
            await Conexion.ExecuteAsync("PSCrearEmpresa", parametros, commandType: CommandType.StoredProcedure); // "PSCrearEmpresa" SP para crear empresa
        }
        catch (System.Exception)
        {
            throw new Exception("Esta empresa ya se encuentra Registrada");
        }   
    } //Check Funcionando 24/06

    public async Task<Empresa>? Detalle(uint indiceABuscar)
    {
        var query = @"Select * From Empresa where idEmpresa = @IndiceEmpresa;";
        var Resultado = await Conexion.QueryFirstOrDefaultAsync<Empresa>(query, new { IndiceEmpresa = indiceABuscar });
        return Resultado;
    } //Check Funcionando 24/06

    public async Task EliminarAdministrador(int xidadministrador)
    {
        var parametros = new DynamicParameters();
        parametros.Add("xidAdministrador", xidadministrador);
        try
        {
            await Conexion.ExecuteAsync("SPDelAdministrador", parametros);
        }
        catch (System.Exception)
        {
            throw new Exception("¡Error al eliminar al administrador!");
        }    
    } //Check Funcionando 24/06

    public async Task EliminarEmpresa(int idempresa)
    {
        var parametros = new DynamicParameters();
        parametros.Add("@xidEmpresa", idempresa);
        try
        {
            await Conexion.ExecuteAsync("SPDelEmpresa", parametros);
        }
        catch (System.Exception)
        {
            throw new Exception("¡Error al eliminar al administrador!");
        }    
    } //Check Funcionando 24/06

    public async Task<IEnumerable<Pedido>> ObtenerPedidos(int xidEmpresa)
    {
        var query = @"Select * 
                        From Pedido 
                        Where EmpresaDestino = @IdEmpresa
                        ORDER BY FechaDespacho DESC";
        var resultados = await Conexion.QueryAsync<Pedido>(query, new {IdEmpresa = xidEmpresa});
        return resultados;
    } //Check Funcionando 24/06

    public Task<Empresa?> ObtenerPorNombre(string Nombre)
    {
        var Query = "Select Nombre From Empresa where Nombre = @InNombreEmpresa;";
        var resultado = Conexion.QueryFirstOrDefaultAsync<Empresa>(Query, new {InNombreEmpresa = Nombre}); 
        return resultado;   
    } //Check Funcionando 24/06
}
