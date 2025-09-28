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
    public async Task TestListarConductoresLibres()
    {
        var resultado = await _RepoConductor.ListarConductoresLibres(); //Check Funcionando 27/06
        Assert.NotNull(resultado);
    }
}
