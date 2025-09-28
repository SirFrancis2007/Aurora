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
    {
        var nuevo_objecto = FixtureAurora.NuevoPedido;
        await _repoPedido.AltaAsync(nuevo_objecto); 

        var vef_nuevo_objecto = await _repoPedido.DetalleAsync(nuevo_objecto.IdPedido);
        Assert.NotNull(vef_nuevo_objecto);
    }//Check Funcionando 26/06

    [Fact]
    // Este es por id
    public async Task Detalle()
    {
        var detalle = await _repoPedido.DetalleAsync(3);
        Assert.NotNull(detalle);
        Assert.Equal(3, detalle.IdPedido);
    } //Check Funcionando 26/06

    [Fact]
    public async Task Obtener()
    {
        Assert.NotNull(await  _repoPedido.ObtenerAsync);
        Assert.NotEmpty(await _repoPedido.ObtenerAsync);
    } //Check Funcionando 26/06
}
