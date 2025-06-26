using System.Threading.Tasks;
using Aurora.Core;
using Aurora.Core.Interfaces;
using Aurora.Dapper.ADO;
using Xunit.Sdk;

namespace Aurora.Dapper.Test;

public class TestConductor : TestBase
{
    IRepoConductor _RepoConductor;
    public TestConductor() : base() => _RepoConductor = new RepoConductor(Conexion);

    [Fact]
    public async Task AltaConductor()
    {
        await _RepoConductor.Alta(FixtureAurora.NuevoConductor);
    } //Check funcionando 26/06

    [Fact]
    public async Task Obtener()
    {
        var Resultados = await _RepoConductor.Obtener;
        Assert.NotNull(Resultados);
    } //Check funcionando 26/06

    [Fact]
    public async Task TestDetallePorIdConductor ()
    {
        var Resultado = await _RepoConductor.Detalle(1); //IdConductor = 1
        Assert.NotNull(Resultado);
        Assert.Equal("Samuel", Resultado.Name);
    } //Check Funcionando 26/06

    [Fact]
    public async Task AsignarVehiculo()
        => await _RepoConductor.AsignarVehiculo(1,1);

    [Fact]
    public async Task DesasignarVehiculo()
        => await _RepoConductor.DesasignarVehiculoDeConductor(1,1);

    [Fact]
    public async Task TestDisponibilidad()
    {
        await _RepoConductor.VerDisponibilidad(1);
    }

    [Fact]
    public async Task TestVefLicencia()
    {
        await _RepoConductor.VefLicencia(FixtureAurora.NuevoConductor.Licencia, FixtureAurora.NuevoConductor.IdConductor, 1);
    }
}
