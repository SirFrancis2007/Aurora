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
        var nombreEsperado = "Porche SRC";
        var InstEmpresa = new Empresa
        {
            Nombre = nombreEsperado
        };

        await ConRepoEmpresa.AltaAsync(InstEmpresa);

        var EmpresaNueva = await ConRepoEmpresa.ObtenerPorNombreAsync(nombreEsperado);
        Assert.Equal(nombreEsperado, EmpresaNueva.Nombre);
    } //Check Funcionando 24/06

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

    [Fact]
    public async Task TestListaPedidoEmpresa()
    {
        var pedidos = await ConRepoEmpresa.ObtenerPedidosAsync(2);
        Assert.NotNull(pedidos);
    } //Check Funcionando 24/06
}
