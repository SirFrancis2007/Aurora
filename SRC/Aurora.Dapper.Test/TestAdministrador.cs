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
        => Assert.NotEmpty(await _repo.Obtener);

    [Fact]
    public async Task TestAltaAdmin()
    {
        Administrador NuevoAdmin = new (){
            IdAdministrador = 0,
            Nombre = "pepe",
            IdEmpresa = 1,
            Password = "1234asd"
        };

        await _repo.Alta(NuevoAdmin);
    }

    [Fact]
    public async Task TestObtenerXAdmin ()
    {
        var Admin = await _repo.Detalle(1);
        Assert.Equal("pepe", Admin.Nombre);
    }
}
