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

    void TestCrearEmpresa()
    {
        var InstEmpresa = new Core.Empresa{
            IdEmpresa = 0,
            Nombre = "Test1 SRC"
        };

        ConRepoEmpresa.Alta(InstEmpresa);
    }

    [Fact]
    void TestCrearAdmin()
    {
        var InstAdmin = new Administrador
        {
            IdAdministrador = 0,
            Nombre = "Pepe",
            Password = "1234",
            IdEmpresa = 1
        };

        ConRepoEmpresa.AgregarAdministrador(InstAdmin);
    }

    [Fact]
    public void TestDetalle()
    {
        var resultado = ConRepoEmpresa.Detalle(1);
        Assert.NotNull(resultado);
        Assert.Equal(1, (double)resultado.IdEmpresa);
    }

    [Fact]
    public void TestListaEmpresa()
    {
        var empresas = ConRepoEmpresa.Obtener();
        Assert.NotNull(empresas);
        Assert.NotEmpty(empresas);
    }

    [Fact]
    public void TestListaPedidoEmpresa()
    {
        var pedidos = ConRepoEmpresa.ObtenerPedidos(1);
        Assert.NotNull(pedidos);
    }
}
