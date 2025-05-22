using Aurora.Core;

namespace Aurora.Dapper.Test;

public class FixtureAurora
{
    // Aca se coloca como prop el tipo de entidad a instanciar
    public static Conductor NuevoConductor => _nuevoConductor;
    static Conductor _nuevoConductor;

    public static Vehiculo NuevoVehiculo => _nuevoVehiculo;
    static Vehiculo _nuevoVehiculo;
    public static Pedido NuevoPedido => _nuevoPedido;
    static Pedido _nuevoPedido;
    public FixtureAurora()
    {
        _nuevoConductor =  new(){
        IdConductor = 0,
        Nombre = "Leo",
        Licencia = "123456789",
        Dispobilidad = true         
        };

        _nuevoVehiculo = new(){
        IdVehiculo = 0,
        Tipo = "Furgoneta",
        CapacidadMax = 150,
        Estado = true,
        Matricula = "AG 16H2 BG"
        };

        _nuevoVehiculo = new(){
        IdVehiculo = 0,
        Tipo = "Camion",
        CapacidadMax = 15000, //Ojota que la unidad es KG pero en tonelada es KG/1000
        Estado = true,
        Matricula = "AG 16H2 BG"
        };

        _nuevoPedido = new(){
            IdPedido = 0,
            NombrePedido = "Plaquetas x30uds",
            Peso = 150, //150 Gramos, 1.5kg
            Volumen = 47, //47lt
            Estado = "Despachado",
            FechaDespacho = DateTime.Now,
            XidAdministrador = 1, //seria pepe
            XidRuta = 1, // seria de Cordoba a BSAS
            XidEmpresa = 2 // Instanciar otra empresa.
        };
    }
}
