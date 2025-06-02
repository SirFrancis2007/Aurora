namespace Aurora.Core.Interfaces;

public interface IRepoConductor : IRepoAlta<Conductor>, IRepoDetalle<Conductor, int>, IRepoListado<Conductor>
{
    public Task EliminarConductor(int idConductor);
    public Task AsignarVehiculo(int conductorId, int vehiculoId);
    public Task  DesasignarVehiculoDeConductor(int conductorId, int vehiculoId);
    public Task<Conductor> VerDisponibilidad(int conductorId);
    // ¡Ojota con este ya que es paso previo al agregar y asginar vehiculo (sirve como doble verificacion)!
    public Task VefLicencia (string Licencia, int idVehiculo, int idConductor);
}
