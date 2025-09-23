namespace Aurora.Core.Models
{
    public class VehiculoConductorDTO
    {
        public int IdConductor { get; set; }
        public string NombreConductor { get; set; }
        public string Licencia { get; set; }
        public int Disponibilidad { get; set; }

        public int? IdVehiculo { get; set; }
        public string Matricula { get; set; }
        public string Tipo { get; set; }
        public double? CapacidadMax { get; set; }
        public int? EstadoVehiculo { get; set; }

        public DateTime? FechaAsignado { get; set; }
    }
}