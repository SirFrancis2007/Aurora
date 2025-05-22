using System.Reflection.Metadata;
using Aurora.Core;
using Aurora.Core.Interfaces;
using Aurora.Dapper.ADO;

namespace Aurora.Dapper.Test;

public class TestVehiculo : TestBase
{
    IRepoVehiculo _repovehiculo;
    public TestVehiculo() : base() => _repovehiculo = new RepoVehiculo(Conexion);
    
    [Fact]
    public void TestNewVehiculo()
        => _repovehiculo.Alta(FixtureAurora.NuevoVehiculo);

    [Fact]
    public void CambiarEstado()
     => Assert.True(_repovehiculo.CambiarEstado(1, false));

    [Fact]
    public void Obtener()
        => Assert.NotNull(_repovehiculo.Obtener());

    [Fact]
    public void ObtenerXid()
        => Assert.NotNull(_repovehiculo.Detalle(1));

    [Fact]
    public void TestEliminarVehiculo()
        => Assert.True(_repovehiculo.EliminarVehiculo(1));

    [Fact]
    public void TestListarPedidosAsignados()
    {
        // Va a traer los distintos paquetes que posee en su tatalidad el vehiculo que lo trasporta.
        Assert.NotEmpty(_repovehiculo.ListarPedidosAsignados(1));
        Assert.NotNull(_repovehiculo.ListarPedidosAsignados(1));
    }
}
