using Aurora.Core.Interfaces;
using Aurora.Dapper.ADO;

namespace Aurora.Dapper.Test;

public class TestConductor : TestBase
{
    IRepoConductor _RepoConductor;
    public TestConductor() : base() => _RepoConductor = new RepoConductor(Conexion);

    [Fact]
    public async Task AltaConductor()
    {
        var objetonuevo = FixtureAurora.NuevoConductor;
        await _RepoConductor.AltaAsync(objetonuevo);

        var obtenerNuevoObjecto = await _RepoConductor.DetalleAsync(objetonuevo.IdConductor);
        Assert.NotNull(obtenerNuevoObjecto);
    } //Check funcionando 26/06

    [Fact]
    public async Task Obtener()
    {
        var Resultados = await _RepoConductor.ObtenerAsync;
        Assert.NotNull(Resultados);
    } //Check funcionando 26/06

    [Fact]
    public async Task TestDetallePorIdConductor ()
    {
        var Resultado = await _RepoConductor.DetalleAsync(1); //IdConductor = 1
        Assert.NotNull(Resultado);
        Assert.Equal("Samuel", Resultado.Name);
    } //Check Funcionando 26/06

    [Fact]
    public async Task AsignarVehiculo()
    {
        await _RepoConductor.AsignarVehiculoAsync(5,1);

        /*Para verificar ver su disponbilidad*/
        var vef_disponibilidad = await _RepoConductor.VerDisponibilidadAsync(1);
        Assert.NotNull(vef_disponibilidad);
        Assert.Equal(false, vef_disponibilidad.Dispobilidad);
    }  //Check Funcionando 27/06

    [Fact]
    public async Task DesasignarVehiculo()
    {        
        await _RepoConductor.DesasignarVehiculoDeConductorAsync(1,5);
        
        var vef_disponibilidad = await _RepoConductor.VerDisponibilidadAsync(1);
        Assert.NotNull(vef_disponibilidad);
        Assert.Equal(false, vef_disponibilidad.Dispobilidad);
        //Check Funcionando 27/06
    }
    
    [Fact]
    public async Task TestDisponibilidad()
    {
        var resultado = await _RepoConductor.VerDisponibilidadAsync(1); //Check Funcionando 27/06
        Assert.NotNull(resultado);
    }

    [Fact]
    public async Task TestVefLicencia()
    {
        Assert.False(await _RepoConductor.VefLicenciaAsync(FixtureAurora.NuevoConductor.Licencia, FixtureAurora.NuevoConductor.IdConductor, 1));
    }
}
