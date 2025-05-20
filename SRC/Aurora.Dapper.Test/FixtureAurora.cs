using Aurora.Core;

namespace Aurora.Dapper.Test;

public class FixtureAurora
{
    // Aca se coloca como prop el tipo de entidad a instanciar
    public static Conductor NuevoConductor => _nuevoConductor;
    static Conductor _nuevoConductor;

    public static Vehiculo NuevoVehiculo => _nuevoVehiculo;
    static Vehiculo _nuevoVehiculo;
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
    }
}
