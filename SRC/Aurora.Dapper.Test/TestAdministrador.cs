using Aurora.Core;
using Aurora.Core.Interfaces;
using Aurora.Dapper.ADO;

namespace Aurora.Dapper.Test;

public class TestAdministrador :  TestBase
{
    IRepoAdministrador _repo; 
    public TestAdministrador() :  base()
        => _repo = new RepoAdministrador(Conexion);

    [Fact]
    public async Task ObtenerAdminOK()
        => Assert.NotEmpty(await _repo.ObtenerAsync);

    [Fact]
    public async Task TestAltaAdmin()
    {
        var NuevoObjecto = FixtureAurora.NuevoAdministrador;
        await _repo.AltaAsync(NuevoObjecto);

        var obtenerNuevoObjecto = _repo.ObtenerDataXidAsync(NuevoObjecto.IdAdministrador);
        Assert.NotNull(obtenerNuevoObjecto);
    }

    [Fact]
    public async Task TestObtenerXAdmin ()
    {
        var admin = await _repo.DetalleAsync(1);
        Assert.Equal(1, admin.IdAdministrador);
    }
}
