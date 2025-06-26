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
        =>await _repoPedido.Alta(FixtureAurora.NuevoPedido); //Check Funcionando 26/06


    [Fact]
    public async Task ObtenerPedidoXCondicion()
    {
        var fecha = DateTime.Today;
        var resultado = await _repoPedido.ObtenerPedidoXCondicion(fecha);
        Assert.NotNull(resultado);
        Assert.Equal(fecha, resultado.FechaDespacho);
    } //Check Funcionando 26/06

    [Fact]
    // Este es por id
    public async Task Detalle()
    {
        var detalle = await _repoPedido.Detalle(3);
        Assert.NotNull(detalle);
        Assert.Equal(3, detalle.IdPedido);
    } //Check Funcionando 26/06

    [Fact]
    public async Task Obtener()
    {
        Assert.NotNull(await  _repoPedido.Obtener);
        Assert.NotEmpty(await _repoPedido.Obtener);
    } //Check Funcionando 26/06
}
