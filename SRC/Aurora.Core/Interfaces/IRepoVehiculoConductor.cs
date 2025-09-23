using System.Numerics;
using Aurora.Core.Models;

namespace Aurora.Core.Interfaces;

public interface IRepoVehiculoConductor : IRepoAlta<VehiculoConductor>, IRepoDetalle<VehiculoConductor, int>, IRepoListado<VehiculoConductor>
{
    // solo se da el alta para la asignarcion de conductor a vehiculo
    public Task<IEnumerable<VehiculoConductorDTO>> Consulta();
}