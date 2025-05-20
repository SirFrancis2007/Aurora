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
    {

    }

    [Fact]
    public void Obtener()
        => Assert.NotNull(_repovehiculo.Obtener());
    

    [Fact]
    public void ObtenerXid()
        => Assert.NotNull(_repovehiculo.Detalle(1));

    [Fact]
    public void TestEliminarVehiculo()
    {
        _repovehiculo.EliminarVehiculo(2);
        Assert.True(true);
    }

    [Fact]
    public void TestListarPedidosAsignados()
    {
        // Va a traer los distintos paquetes que posee en su tatalidad el vehiculo que lo trasporta.

    }
}
