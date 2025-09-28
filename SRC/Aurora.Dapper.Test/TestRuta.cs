using Aurora.Core;
using Aurora.Core.Interfaces;
using Aurora.Core;
using Aurora.Dapper.ADO;
using System.Threading.Tasks;

namespace Aurora.Dapper.Test;

public class TestRuta : TestBase
{
    IRepoRuta repoRuta;
    public TestRuta() : base()
        => repoRuta = new RepoRuta(Conexion);

    [Fact]
    public async Task TestCreateRuta()
    {
        var nuevo_objecto = FixtureAurora.NuevaRuta;
        await repoRuta.AltaAsync(nuevo_objecto);

        var vef_nuevo_objecto = await repoRuta.DetalleAsync(nuevo_objecto.IdRuta);
        Assert.NotNull(vef_nuevo_objecto);
    } //Check Funcionando 26/06

    [Fact]
    public async Task TestObtenerOk()
    {
        var Resultado = await repoRuta.ObtenerAsync;
        Assert.NotEmpty(Resultado);
    } //Check Funcionando 26/06

    [Fact]
    public async Task TestObtenerxIdOk()
    {
        var Resultado = await repoRuta.ObtenerRutaPorParametrosAsync("Cordoba", "Buenos Aires", 1);
        Assert.NotEmpty(Resultado.Origen);
    } //Check Funcionando 26/06
}
