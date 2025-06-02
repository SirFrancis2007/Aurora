using System.Threading.Tasks;
using Aurora.Core.Interfaces;
using Aurora.Dapper.ADO;

namespace Aurora.Dapper.Test;

public class TestPedido : TestBase
{
    IRepoPedido _repoPedido;
    public TestPedido() => _repoPedido = new RepoPedido(Conexion);

    [Fact]
    public async Task AltaPedido()
        =>await _repoPedido.Alta(FixtureAurora.NuevoPedido);


    [Fact]
    public async Task ObtenerPedidoXCondicion()
    {
        var fecha = DateTime.Today;
        var resultado = await _repoPedido.ObtenerPedidoXCondicion(fecha);
        Assert.NotNull(resultado);
        Assert.Equal(fecha, resultado.FechaDespacho);
    }

    [Fact]
    public async Task AsignarVehiculo()
    {
        await _repoPedido.AsignarVehiculo(1, 1);
    }

    [Fact]
    // Este es por id
    public async Task Detalle()
    {
        Assert.NotNull(await _repoPedido.Detalle(1));
        var detalle = await _repoPedido.Detalle(1);
        Assert.Equal(1, detalle.IdPedido);
    }

    [Fact]
    public async Task Obtener()
    {
        Assert.NotNull(await  _repoPedido.Obtener);
        Assert.NotEmpty(await _repoPedido.Obtener);
    }
}
