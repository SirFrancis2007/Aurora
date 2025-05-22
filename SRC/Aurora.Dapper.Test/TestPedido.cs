using Aurora.Core.Interfaces;
using Aurora.Dapper.ADO;

namespace Aurora.Dapper.Test;

public class TestPedido : TestBase
{
    IRepoPedido _repoPedido;
    public TestPedido() => _repoPedido = new RepoPedido(Conexion);

    [Fact]
    public void AltaPedido()
        =>_repoPedido.Alta(FixtureAurora.NuevoPedido);


    [Fact]
    public void ObtenerPedidoXCondicion()
    {
        var fecha = DateTime.Today;
        var resultado = _repoPedido.ObtenerPedidoXCondicion(fecha);
        Assert.NotNull(resultado);
        Assert.Equal(fecha, resultado.FechaDespacho);
    }

    [Fact]
    public void ActualizarEstado()
    {
        // se utiliza indirectamente la exepcion suponiendo que si no tira la misma es porque funca, por eso debe ser nulo.
        Exception ex = Record.Exception(() => _repoPedido.ActualizarEstado(1, "En viaje"));
        Assert.Null(ex);
    }

    [Fact]
    public void AsignarVehiculo()
    {
        Exception ex = Record.Exception(() => _repoPedido.AsignarVehiculo(1, 1));
        Assert.Null(ex);
    }

    [Fact]
    // Este es por id
    public void Detalle()
    {
        Assert.NotNull(_repoPedido.Detalle(1));
        Assert.Equal(1, _repoPedido.Detalle(1).IdPedido);
    }

    [Fact]
    public void Obtener()
    {
        Assert.NotNull( _repoPedido.Obtener());
        Assert.NotEmpty( _repoPedido.Obtener());
    }
}
