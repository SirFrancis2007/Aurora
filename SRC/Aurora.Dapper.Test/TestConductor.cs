using Aurora.Core;
using Aurora.Core.Interfaces;
using Aurora.Dapper.ADO;
using Xunit.Sdk;

namespace Aurora.Dapper.Test;

public class TestConductor : TestBase
{
    IRepoConductor _RepoConductor;
    public TestConductor() : base() => _RepoConductor = new RepoConductor(Conexion);

    [Fact]
    public void AltaConductor()
    {
        _RepoConductor.Alta(FixtureAurora.NuevoConductor);
    }

    [Fact]
    public void Obtener()
    {
        var Resultados = _RepoConductor.Obtener();
        Assert.NotNull(Resultados);
    }

    [Fact]
    public void ObtenerXId ()
    {
        var Resultado = _RepoConductor.Detalle(1);
        Assert.NotNull(Resultado);
        Assert.Equal("pepe", Resultado.Nombre);
    }

    [Fact]
    public void AsignarVehiculo()
        => _RepoConductor.AsignarVehiculo(1,1);

    [Fact]
    public void DesasignarVehiculo()
        => _RepoConductor.DesasignarVehiculoDeConductor(1,1);

    [Fact]
    public void TestDisponibilidad()
    {
        _RepoConductor.VerDisponibilidad(1);
    }

    [Fact]
    public void TestVefLicencia()
    {
        _RepoConductor.VefLicencia(FixtureAurora.NuevoConductor.Licencia, FixtureAurora.NuevoConductor.IdConductor, 1);
    }
}
