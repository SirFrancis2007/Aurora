using System.Reflection.Metadata;
using System.Threading.Tasks;
using Aurora.Core;
using Aurora.Core.Interfaces;
using Aurora.Dapper.ADO;

namespace Aurora.Dapper.Test;

public class TestVehiculo : TestBase
{
    IRepoVehiculo _repovehiculo;
    public TestVehiculo() : base() => _repovehiculo = new RepoVehiculo(Conexion);
    
    [Fact]
    public async Task TestNewVehiculo()
        => await _repovehiculo.Alta(FixtureAurora.NuevoVehiculo); // Check 24/06

    [Fact]
    public async Task CambiarEstado()
        => Assert.True(await _repovehiculo.CambiarEstado(1, false)); // Check 24/06

    [Fact]
    public async Task Obtener()
        => Assert.NotNull(await _repovehiculo.Obtener); // Check 24/06

    [Fact]
    public async Task ObtenerXid()
        => Assert.NotNull(await _repovehiculo.Detalle(1)); // Check 24/06

    [Fact]
    public async Task TestEliminarVehiculo()
        => Assert.False(await _repovehiculo.EliminarVehiculo(1));

    [Fact]
    public async Task TestListarPedidosAsignados()
    {
        // Va a traer los distintos paquetes que posee en su totalidad el vehiculo que lo trasporta.
        Assert.NotEmpty((IAsyncEnumerable<Pedido>)await _repovehiculo.ListarPedidosAsignados(1));
        Assert.NotNull(await _repovehiculo.ListarPedidosAsignados(1));
    }
}
