using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinimalAPI.DTO;

public struct AdministradoresDTO
{
    public uint IdEmpresa {get; set;}
    public required string Nombre { get; set; }
    public required string Password { get; set; }
}
