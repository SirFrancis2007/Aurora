using Aurora.Core.Models;

namespace Aurora.Core.Interfaces;

public interface IRepoVehiculo : IRepoAlta<Vehiculo>, IRepoDetalle<Vehiculo, int>, IRepoListado<Vehiculo>
{

    /// <summary>
    /// Elimina un vehículo según su Id.
    /// </summary>
    /// <param name="id">Identificador del vehículo.</param>
    /// <returns>True si fue eliminado, False si no existe.</returns>
    public Task<bool> EliminarVehiculoAsync(int idVehiculo);

    /// <summary>
    /// Actualiza el estado del vehiculo
    /// </summary>
    /// <param name="disponible">Estado.</param>
    /// <param name="idvehiculo">Identificador del vehículo.</param>
    /// <returns>True si fue modificado, False si no fue modificado.</returns> 
    public Task<Boolean> CambiarEstadoAsync(int vehiculoId, bool disponible);

    public Task<IEnumerable<VehiculoDTO>> ObtenerVehiculosDisponibles();

    public Task<bool> ActualizarEstadoVehiculo(int id);
}
