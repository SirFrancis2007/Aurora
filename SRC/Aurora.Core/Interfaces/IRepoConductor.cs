namespace Aurora.Core.Interfaces;

public interface IRepoConductor : IRepoAlta<Conductor>, IRepoDetalle<Conductor, int>, IRepoListado<Conductor>
{
    public Task EliminarConductorAsync(int idConductor);
    public Task AsignarVehiculoAsync(int conductorId, int vehiculoId);
    public Task DesasignarVehiculoDeConductorAsync(int conductorId, int vehiculoId);
    public Task<Conductor> VerDisponibilidadAsync(int conductorId);
    // ¡Ojota con este ya que es paso previo al agregar y asginar vehiculo (sirve como doble verificacion)!
    public Task<bool> VefLicenciaAsync(string Licencia, int idVehiculo, int idConductor);
    public Task<Boolean> UpdateConductorAsync(Conductor conductor);
    public Task<List<Conductor>> ListarConductoresSinVehiculoAsync(); //Conductores libres, estado = 1 (true)
}
