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
    public void ObtenerAdminOK()
        => Assert.NotEmpty(_repo.Obtener());

    [Fact]
    public void TestAltaAdmin()
    {
        Administrador NuevoAdmin = new (){
            IdAdministrador = 0,
            Nombre = "pepe",
            IdEmpresa = 1,
            Password = "1234asd"
        };

        _repo.Alta(NuevoAdmin);
    }

    [Fact]
    public void TestObtenerXAdmin ()
    {
        var Admin = _repo.Detalle(1);
        Assert.Equal("pepe", Admin.Nombre);
    }
}
