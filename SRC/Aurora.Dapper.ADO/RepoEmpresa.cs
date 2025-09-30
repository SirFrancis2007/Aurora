using System.Data;
using Aurora.Core;
using Aurora.Core.Interfaces;
using Dapper;

namespace Aurora.Dapper.ADO;

public class RepoEmpresa : RepoGenerico, IRepoEmpresa
{
    private uint _idempresanueva;
    Task<IEnumerable<Empresa>> IRepoListado<Empresa>.ObtenerAsync => Obtener();

    public RepoEmpresa(IDbConnection conexion) : base(conexion) { }

    public async Task<IEnumerable<Empresa>> Obtener()
    {
        var query = @"Select * From Empresa";
        var Resultado = await Conexion.QueryAsync<Empresa>(query);
        return Resultado;
    }

    public async Task<bool> LoguearseAsync(string nombre, string contrasena)
    {
        var funtionLogin = "SELECT FLoginEmpresa(@xNombre, @xContrasena);";
        var result = await Conexion.ExecuteScalarAsync<bool>(funtionLogin, new { xNombre = nombre, xContrasena = contrasena });
        return result;

    }

    public async Task<bool> EliminarEmpresaAsync(uint idempresa)
    {
        var parametros = new DynamicParameters();
        parametros.Add("@xidEmpresa", idempresa);
        try
        {
            var resultado = await Conexion.ExecuteAsync("SPDelEmpresa", parametros);
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
            throw new Exception("¡Error al eliminar al administrador!");
        }
    }

    public async Task<Empresa?> ObtenerPorNombreAsync(string nombre)
    {
        var Query = "Select idEmpresa From Empresa where Nombre = @InNombreEmpresa;";
        var resultado = await Conexion.QueryFirstOrDefaultAsync<Empresa>(Query, new { InNombreEmpresa = nombre });
        return resultado;
    }

    public async Task AltaAsync(Empresa NuevaEmpresa)
    {
        var parametros = new DynamicParameters();
        parametros.Add("xidEmpresa", direction: ParameterDirection.Output);
        parametros.Add("xNombre", NuevaEmpresa.Nombre);
        parametros.Add("xContrasena", NuevaEmpresa.Contrasena);
        try
        {
            await Conexion.ExecuteAsync("PSCrearEmpresa", parametros, commandType: CommandType.StoredProcedure); // "PSCrearEmpresa" SP para crear empresa
            _idempresanueva = (uint)parametros.Get<int>("xidEmpresa");
        }
        catch (System.Exception ex)
        {
            throw new Exception($"Error al crear empresa: {ex.Message}", ex);
        }
    }

    public async Task<Empresa>? DetalleAsync(uint indiceABuscar)
    {
        var query = @"Select * From Empresa where idEmpresa = @IndiceEmpresa;";
        var Resultado = await Conexion.QueryFirstOrDefaultAsync<Empresa>(query, new { IndiceEmpresa = indiceABuscar });
        return Resultado;

    }
}
