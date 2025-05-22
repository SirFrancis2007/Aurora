using System.Data;
using Aurora.Core;
using Aurora.Core.Interfaces;
using Dapper;

namespace Aurora.Dapper.ADO;

public class RepoHistorialPedido : RepoGenerico, IRepoHisrorialPedido
{
    public RepoHistorialPedido(IDbConnection conexion) : base(conexion)
    {
    }

    /*ojota que no se da de alta ni se elimina ya que eso ya lo lleva a cabo la bd*/

    public List<HistorialPedido> ObtenerHistorialPorPedido(int xpedidoid)
    {
        var Query = "SELECT * FROM HistorialPedido WHERE Pedido_idPedido = @xpedidoid ORDER BY FechaCambio DESC";
        var Repuesta = Conexion.Query<HistorialPedido>(Query).ToList();
        return Repuesta;
    }
}
