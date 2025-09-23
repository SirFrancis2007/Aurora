namespace Aurora.Core;

public class HistorialDTO
{
    public int idPedido { get; set; }
    public string NombrePedido { get; set; }
    public int Volumen { get; set; }
    public int Peso { get; set; }
    public string EstadoPedido { get; set; }
    public DateTime FechaDespacho { get; set; }
    public string TipoVehiculo { get; set; }
    public string Matricula { get; set; }
    public string Conductor { get; set; }
    public string EmpresaOrigen { get; set; }
    public int idEmpresaOrigen { get; set; }
    public string Origen { get; set; }
    public string Destino { get; set; }
    public string EmpresaDestino { get; set; }
    public string EstadoAnterior { get; set; }
    public string EstadoNuevo { get; set; }
    public DateTime FechaCambio { get; set; }
}
