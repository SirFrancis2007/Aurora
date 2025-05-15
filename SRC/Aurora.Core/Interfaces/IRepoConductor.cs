namespace Aurora.Core.Interfaces;

public interface IRepoConductor
{
    public void CrearConductor(Conductor NewConductor);
    public void EliminarConductor(int idConductor);
    public void AsignarVehiculo(int conductorId, int vehiculoId);
    public void DesasignarVehiculoDeConductor(int conductorId, int vehiculoId);
    public Conductor VerDisponibilidad(int conductorId);
    public bool VefLicencia (string Licencia);
}
