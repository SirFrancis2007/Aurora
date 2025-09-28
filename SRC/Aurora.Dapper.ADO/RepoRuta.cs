using System.Data;
using Aurora.Core;
using Aurora.Core.Interfaces;
using Dapper;

namespace Aurora.Dapper.ADO;

public class RepoRuta : RepoGenerico, IRepoRuta
{
    public RepoRuta(IDbConnection conexion) : base(conexion) {}

    public Task<IEnumerable<Ruta>> ObtenerAsync => ObtenerData();
    public async Task<IEnumerable<Ruta>> ObtenerData()
    {
        var query = @"Select * from Ruta";
        var Resultados = await Conexion.QueryAsync<Ruta>(query);
        return Resultados;
    }

    public async Task AltaAsync(Ruta elemento)
    {
        var parametros = new DynamicParameters();
        parametros.Add("xidRuta", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parametros.Add("xOrigen", elemento.Origen);
        parametros.Add("xDestino", elemento.Destino);

        try
        {
            await Conexion.ExecuteAsync("SPCrearRuta", parametros, commandType: CommandType.StoredProcedure);
            elemento.IdRuta = parametros.Get<int>("xidRuta");
        }
        catch (Exception)
        {
            throw new Exception("¡Error al generar la ruta!");
        }
    }

    public async Task<Ruta>? DetalleAsync(int indiceABuscar)
    {
        var query = @"Select * from Ruta where idRuta = @indiceABuscar";
        var Resultado = await Conexion.QueryFirstOrDefaultAsync<Ruta>(query, new {indiceABuscar});
        return Resultado;
    }


    public async Task<Ruta?> ObtenerRutaPorParametrosAsync(string origen, string destino, int idRuta)
    {
        var query = @"Select * from Ruta where idRuta = @Indice or Origen = @xOrigen or Destino = @xDestino";
        var Resultado = await Conexion.QueryFirstOrDefaultAsync<Ruta>(query, new {Indice = idRuta, xOrigen = origen, xDestino = destino});
        return Resultado;
    }
}
