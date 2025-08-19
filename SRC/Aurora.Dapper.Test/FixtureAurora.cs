using Aurora.Core;

namespace Aurora.Dapper.Test;

public class FixtureAurora
{
    // Aca se coloca como prop el tipo de entidad a instanciar
    public static Empresa NuevaEmpreas => _nuevaempresa;
    static Empresa _nuevaempresa;
    public static Conductor NuevoConductor => _nuevoConductor;
    static Conductor _nuevoConductor;
    public static Administrador NuevoAdministrador => _nuevoAdministrador;
    static Administrador _nuevoAdministrador;
    public static Vehiculo NuevoVehiculo => _nuevoVehiculo;
    static Vehiculo _nuevoVehiculo;
    public static Pedido NuevoPedido => _nuevoPedido;
    static Pedido _nuevoPedido;
    public static Ruta NuevaRuta => _nuevaruta;
    static Ruta _nuevaruta;
    public FixtureAurora()
    {
        _nuevaempresa = new()
        {
            IdEmpresa = 0,
            Nombre = "Empresa Creado desde Fixture"
        };

        _nuevoConductor =  new(){
            IdConductor = 0,
            Name = "Miguel",
            Licencia = "4h5d5fh",
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
            XidEmpresa = 2, // Instanciar otra empresa.
            xidVehiculo = 1
        };

        _nuevaruta = new Ruta()
        {
            IdRuta = 0,
            Origen = "Cordoba",
            Destino = "Buenos Aires"
        };

        _nuevoAdministrador = new (){
            Nombre = "pepe",
            IdEmpresa = 1,
            Password = "1234asd"
        };
    }
}
