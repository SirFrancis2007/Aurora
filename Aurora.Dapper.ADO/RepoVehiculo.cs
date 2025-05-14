using System.Data;
using Aurora.Core;
using Aurora.Core.Interfaces;

namespace Aurora.Dapper.ADO;

public class RepoVehiculo : RepoGenerico, IRepoVehiculo
{
    public RepoVehiculo(IDbConnection conexion) : base(conexion)
    {
    }

    public void AñadirVehiculo(Vehiculo NewVehiculo)
    {
        throw new NotImplementedException();
    }

    public void CambiarEstado(int vehiculoId, bool disponible)
    {
        throw new NotImplementedException();
    }

    public void EliminarVehiculo(int idVehiculo)
    {
        throw new NotImplementedException();
    }

    public List<Pedido> ListarPedidosAsignados(int vehiculoId)
    {
        throw new NotImplementedException();
    }
}
