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
        => await _repovehiculo.Alta(FixtureAurora.NuevoVehiculo);

    [Fact]
    public async Task CambiarEstado()
     => Assert.True(await _repovehiculo.CambiarEstado(1, false));

    [Fact]
    public async Task Obtener()
        => Assert.NotNull(await _repovehiculo.Obtener);

    [Fact]
    public async Task ObtenerXid()
        => Assert.NotNull(await _repovehiculo.Detalle(1));

    [Fact]
    public async Task TestEliminarVehiculo()
        => Assert.True(await _repovehiculo.EliminarVehiculo(1));

    [Fact]
    public async Task TestListarPedidosAsignados()
    {
        // Va a traer los distintos paquetes que posee en su tatalidad el vehiculo que lo trasporta.
        Assert.NotEmpty((IAsyncEnumerable<Pedido>)await _repovehiculo.ListarPedidosAsignados(1));
        Assert.NotNull(await _repovehiculo.ListarPedidosAsignados(1));
    }
}
