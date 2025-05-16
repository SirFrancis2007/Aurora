namespace Aurora.Core.Interfaces;

public interface IRepoConductor : IRepoAlta<Conductor>, IRepoDetalle<Conductor, int>, IRepoListado<Conductor>
{
    public void EliminarConductor(int idConductor);
    public void AsignarVehiculo(int conductorId, int vehiculoId);
    public void DesasignarVehiculoDeConductor(int conductorId, int vehiculoId);
    public Conductor VerDisponibilidad(int conductorId);
    // ¡Ojota con este ya que es paso previo al agregar y asginar vehiculo (sirve como doble verificacion)!
    public bool VefLicencia (string Licencia, int idVehiculo, int idConductor);
}
