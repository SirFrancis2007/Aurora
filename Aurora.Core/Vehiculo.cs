namespace Aurora.Core;

public class Vehiculo
{
    public int IdVehiculo { get; set; }
    public required string Tipo { get; set; }
    public required double CapacidadMax { get; set; }
    public required bool Estado { get; set; }
}
