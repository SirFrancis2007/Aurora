using System.Data;
using Aurora.Core;
using Aurora.Core.Interfaces;
using Dapper;

namespace Aurora.Dapper.ADO;

public class RepoRuta : RepoGenerico, IRepoRuta
{
    public RepoRuta(IDbConnection conexion) : base(conexion)
    {
    }

    public void Alta(Ruta elemento)
    {
        var parametros = new DynamicParameters();
        parametros.Add("xOrigen", elemento.Origen);
        parametros.Add("xDestino", elemento.Destino);

        try
        {
            Conexion.Execute("SPCrearRuta", parametros);
        }
        catch (System.Exception)
        {
            throw new Exception("¡Error al generar la ruta!");
        }
    }

    public Ruta? Detalle(int indiceABuscar)
    {
        var query = @"Select * from Ruta where idRuta = {indiceABuscar}";
        var Resultado = Conexion.QueryFirstOrDefault<Ruta>(query);
        return Resultado;
    }

    public IEnumerable<Ruta> Obtener()
    {
        var query = @"Select * from Ruta";
        var Resultados = Conexion.Query<Ruta>(query);
        return Resultados;
    }

    public void ObtenerRutaPorCondicion(int? idRuta, string xOrigen, string xDestino)
    {
        var query = @"Select * from Ruta where idRuta = {indiceABuscar} or Origen = {xOrigen} or Destino = {xDestino}";
        var Resultado = Conexion.QueryFirstOrDefault<Ruta>(query);
    }
}
