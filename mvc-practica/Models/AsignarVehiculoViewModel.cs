using Aurora.Core;

public class AsignarVehiculoViewModel
{
    // Datos de Conductor
    public int IdConductor { get; set; }
    public string NombreConductor { get; set; }
    public string Licencia { get; set; }
    public int Disponibilidad { get; set; }

    // Datos de Vehículo
    public int? IdVehiculo { get; set; }
    public string Matricula { get; set; }
    public string Tipo { get; set; }
    public double? CapacidadMax { get; set; }
    public int? Estado { get; set; }

    public DateTime? FechaAsignado { get; set; }
}