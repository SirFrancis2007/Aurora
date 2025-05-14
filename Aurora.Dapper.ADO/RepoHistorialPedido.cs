using System.Data;
using Aurora.Core;
using Aurora.Core.Interfaces;

namespace Aurora.Dapper.ADO;

public class RepoHistorialPedido : RepoGenerico, IRepoHisrorialPedido
{
    public RepoHistorialPedido(IDbConnection conexion) : base(conexion)
    {
    }

    public List<HistorialPedido> ObtenerHistorialPorPedido(int pedidoId)
    {
        throw new NotImplementedException();
    }

    public void RegistrarCambio(int pedidoId, string estadoAnterior, string estadoNuevo, DateTime fechaCambio)
    {
        throw new NotImplementedException();
    }
}
