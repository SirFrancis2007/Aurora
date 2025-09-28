using Aurora.Core;
using Aurora.Core.Interfaces;
using Aurora.Dapper.ADO;

namespace Aurora.Dapper.Test;

public class TestEmpresa : TestBase
{
    readonly IRepoEmpresa ConRepoEmpresa;

    public TestEmpresa() : base()
    {
        ConRepoEmpresa = new RepoEmpresa(Conexion);
    }

    [Fact]

    public async Task TestCrearEmpresa()
    {
        await ConRepoEmpresa.AltaAsync(FixtureAurora.NuevaEmpreas);

        var EmpresaNueva = await ConRepoEmpresa.ObtenerPorNombreAsync(FixtureAurora.NuevaEmpreas.Nombre);
        Assert.Equal(FixtureAurora.NuevaEmpreas.Nombre, EmpresaNueva.Nombre);
    } //Check Funcionando 19/8 Y Forkeado

    [Fact]
    public async Task TestDetalle()
    {
        var resultado = await ConRepoEmpresa.DetalleAsync(1);
        Assert.NotNull(resultado);
        Assert.Equal(1, (double)resultado.IdEmpresa);
    } //Check Funcionando 24/06

    [Fact]
    public async Task TestListaEmpresa()
    {
        var empresas = await ConRepoEmpresa.ObtenerAsync;
        Assert.NotNull(empresas);
        Assert.NotEmpty(empresas);
    } //Check Funcionando 24/06
}
