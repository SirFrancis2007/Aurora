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
        Ruta NuevaRuta = new Ruta()
        {
            IdRuta = 0,
            Origen = "Cordoba",
            Destino = "Buenos Aires"
        };

        //Llamar a dar de alta la ruta.
        await repoRuta.Alta(NuevaRuta);
    }

    [Fact]
    public async Task TestObtenerOk()
    {
        var Resultado = await repoRuta.Obtener;
        Assert.NotEmpty(Resultado);
    }

    [Fact]
    public async Task TestObtenerxIdOk()
    {
        var Resultado = await repoRuta.ObtenerRutaPorCondicion(1, "Cordoba", "Buenos Aires");
        Assert.NotEmpty(Resultado.Origen);
    } 
}
