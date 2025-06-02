using System.Data;
using Aurora.Core;
using Aurora.Core.Interfaces;
using Dapper;

namespace Aurora.Dapper.ADO;

public class RepoEmpresa : RepoGenerico, IRepoEmpresa
{
    public RepoEmpresa(IDbConnection conexion) : base(conexion)
    {
    }

    public Task<IEnumerable<Empresa>> Obtener => throw new NotImplementedException();

    public async Task Alta(Empresa NuevaEmpresa)
    {
        var parametros = new DynamicParameters();
        parametros.Add("xNombre", NuevaEmpresa.Nombre);
        try
        {
            await Conexion.ExecuteAsync("PSCrearEmpresa", parametros); // "PSCrearEmpresa" SP para crear empresa
        }
        catch (System.Exception)
        {
            throw new Exception("Esta empresa ya se encuentra Registrada");
        }   
    }

    public async Task<Empresa>? Detalle(uint indiceABuscar)
    {
        var query = @"Select * From Empresa where idEmpresa = {indiceABuscar}";
        var Resultado = await Conexion.QueryFirstOrDefaultAsync<Empresa>(query);
        return Resultado;
    }

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
    }

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
    }

    public async Task<IEnumerable<Pedido>> ObtenerPedidos(int xidEmpresa)
    {
        var query = @"Select * 
                        From Pedido 
                        Where p.EmpresaDestino = {xidEmpresa} OR a.Empresa_idEmpresa = {xidEmpresa} 
                        ORDER BY p.FechaDespacho DESC";
        var resultados = await Conexion.QueryAsync<Pedido>(query);
        return resultados;
    }

    public IEnumerable<Empresa> ObtenerDatos()
    {
        var query = @"Select * From Empresa";
        var Resultado = Conexion.Query<Empresa>(query);
        return Resultado;
    }
}
