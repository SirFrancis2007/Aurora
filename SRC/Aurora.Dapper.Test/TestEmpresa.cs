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
        var InstEmpresa = new Core.Empresa{
            IdEmpresa = 0,
            Nombre = "Test1 SRC"
        };

        await ConRepoEmpresa.Alta(InstEmpresa);
    }

    [Fact]
    public async Task TestDetalle()
    {
        var resultado = await ConRepoEmpresa.Detalle(1);
        Assert.NotNull(resultado);
        Assert.Equal(1, (double)resultado.IdEmpresa);
    }

    [Fact]
    public async Task TestListaEmpresa()
    {
        var empresas = await ConRepoEmpresa.Obtener;
        Assert.NotNull(empresas);
        Assert.NotEmpty(empresas);
    }

    [Fact]
    public async Task TestListaPedidoEmpresa()
    {
        var pedidos = await ConRepoEmpresa.ObtenerPedidos(1);
        Assert.NotNull(pedidos);
    }
}
