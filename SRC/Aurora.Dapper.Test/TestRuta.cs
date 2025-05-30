using Aurora.Core;
using Aurora.Core.Interfaces;
using Aurora.Core;
using Aurora.Dapper.ADO;

namespace Aurora.Dapper.Test;

public class TestRuta : TestBase
{
    IRepoRuta repoRuta;
    public TestRuta() : base()
        => repoRuta = new RepoRuta(Conexion);

    [Fact]
    public void TestCreateRuta()
    {
        Ruta NuevaRuta = new Ruta()
        {
            IdRuta = 0,
            Origen = "Madrid",
            Destino = "Lisboa"
        };

        //Llamar a dar de alta la ruta.
        repoRuta.Alta(NuevaRuta);
    }

    [Fact]
    public void TestObtenerOk()
    {
        var Resultado = repoRuta.Obtener();
        Assert.NotEmpty(Resultado);
    }

    [Fact]
    public void TestObtenerxIdOk()
    {
        var Resultado = repoRuta.ObtenerRutaPorCondicion(1, "Cordoba", "Buenos Aires");
        Assert.NotEmpty(Resultado.Origen);
    } 
}
