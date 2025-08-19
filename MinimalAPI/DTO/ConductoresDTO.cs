using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinimalAPI.DTO;

public struct ConductoresDTO
{
    public int IdConductor { get; set; }
    public required string Name { get; set; }
    public string? Licencia { get; set; }
    public required bool Dispobilidad { get; set; }
}

public class ConductorLicenciaDTO
{
    public int IdConductor { get; set; }
    public string Licencia { get; set; } = string.Empty;
}
