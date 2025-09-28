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
    {
        var objecto_nuevo = FixtureAurora.NuevoVehiculo;
        await _repovehiculo.AltaAsync(objecto_nuevo);

        var vef_nuevo_objecto = await _repovehiculo.DetalleAsync(objecto_nuevo.IdVehiculo);
        Assert.NotNull(vef_nuevo_objecto);
    } // Check 1/7

    [Fact]
    public async Task CambiarEstado()
        => Assert.True(await _repovehiculo.CambiarEstadoAsync(1, false)); // Check 24/06

    [Fact]
    public async Task Obtener()
        => Assert.NotNull(await _repovehiculo.ObtenerAsync); // Check 24/06

    [Fact]
    public async Task ObtenerXid()
        => Assert.NotNull(await _repovehiculo.DetalleAsync(1)); // Check 24/06

    [Fact]
    public async Task TestEliminarVehiculo()
        => Assert.True(await _repovehiculo.EliminarVehiculoAsync(4)); // Agarrar uno que no tenga pedido asignados

    [Fact]
    public async Task TestListarPedidosAsignados()
    {
        var pedidos = await _repovehiculo.DetalleAsync(1);
    
        Assert.NotNull(pedidos);
    } //Check Funcionando 26/06
}
