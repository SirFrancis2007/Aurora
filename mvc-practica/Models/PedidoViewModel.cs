using Aurora.Core;
using Aurora.Core.Models;

public class PedidoViewModel
{
    public required string Titulo { get; set; }
    public required double Volumen { get; set; }
    public required double Peso { get; set; }
    public required string Estado { get; set; }
    public required DateTime FechaDespacho { get; set; }
    public required int IdRuta { get; set; }
    public required int IdEmpresaDestino { get; set; }
    public required int IdVehiculo { get; set; }
    public required int IdAdministrador { get; set; }

    //VAR DE CANTIDAD DE CAPACIDAD RESTANTE: COTEMPLA UN CALCULO DE EN BASE A LA CAPACIDAD MAXIMA - LA SUMATORIA DEL PESO DE LOS PEDIDOS DE ESE VEHICULO

    // Estas lista serviran para almacenar los datos para los select.
    public IEnumerable<Ruta>? Rutas { get; set; }
    public IEnumerable<Empresa>? Empresas { get; set; }
    public IEnumerable<VehiculoDTO>? Vehiculos { get; set; }
}
