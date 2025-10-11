using Aurora.Core.Models;
namespace mvc_practica.Models;

public class AsignacionVehiculoViewModel
{
    public IEnumerable<VehiculoConductorDTO> ConductoresAsignados { get; set; }
    public IEnumerable<VehiculoConductorDTO> ConductoresNoAsignados { get; set; }
    public IEnumerable<VehiculoConductorDTO> VehiculosNoAsignados { get; set; }
}

