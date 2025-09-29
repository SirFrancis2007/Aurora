namespace Aurora.Core;

public class Conductor
{
    public int IdConductor { get; set; }
    public required string Name { get; set; }
    public string? Licencia { get; set; }
    public required bool Disponibilidad { get; set; }
}
