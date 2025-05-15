namespace Aurora.Core;

public class Conductor
{
    public int IdConductor { get; set; }
    public required string Nombre { get; set; }
    public string? Licencia { get; set; }
    public required bool Dispobilidad { get; set; }
}
