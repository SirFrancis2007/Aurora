using System.Numerics;
using Aurora.Core.Models;

namespace Aurora.Core.Interfaces;

public interface IRepoVehiculoConductor : IRepoAlta<VehiculoConductor>, IRepoDetalle<VehiculoConductor, int>, IRepoListado<VehiculoConductor>
{
    /// <summary>
    /// Asigna un conductor a un vehículo.
    /// </summary>
    /// <param name="idVehiculo">Id del vehículo.</param>
    /// <param name="idConductor">Id del conductor.</param>
    /// <returns>El registro de asignación creado.</returns>
    /// <exception cref="InvalidOperationException">Si el vehículo o conductor no existen, o si ya está asignado.</exception>
    public Task AsignarConductorAVehiculo(int idVehiculo, int idConductor);
    /// <summary>
    /// Desagsigna al conductor de un vehiculo siempre y cuando este termine el recorrido y no tenga pedidos asignados.
    /// </summary>
    /// <param name="idVehiculo"></param>
    /// <param name="idConductor"></param>
    /// <returns></returns>
    public Task<bool> DesasignarConductorDeVehiculo(int idVehiculo, int idConductor);

    /// <summary>
    /// Condulta sobre los conductores.
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<VehiculoConductorDTO>> consultaVehiculoConductor();

    /// <summary>
    /// Obtiene el id del vehiculo asignado al conductor
    /// </summary>
    /// <param name="idConductor"></param>
    /// <returns></returns>
    public Task<int> ObtenerVehiculoPorIdConductor(int idConductor);
}