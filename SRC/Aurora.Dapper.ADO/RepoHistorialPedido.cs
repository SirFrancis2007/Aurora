using System.Data;
using Aurora.Core;
using Aurora.Core.Interfaces;
using Dapper;

namespace Aurora.Dapper.ADO;

public class RepoHistorialPedido : RepoGenerico, IRepoHisrorialPedido
{
    public RepoHistorialPedido(IDbConnection conexion) : base(conexion) {}

    /*ojota que no se da de alta ni se elimina ya que eso ya lo lleva a cabo la bd*/
    public async Task<List<HistorialPedido>> ObtenerHistorialPorPedidoAsync(int pedidoId)
    {
        var Query = "SELECT * FROM HistorialPedido WHERE Pedido_idPedido = @xpedidoid ORDER BY FechaCambio DESC";
        var Repuesta = await Conexion.QueryAsync<HistorialPedido>(Query);
        return (List<HistorialPedido>)Repuesta;    
    }

    public async Task<IEnumerable<PedidoEmpresaDTO>> ObtenerHistorialesDeEmpresa(int idEmpresa)
    {
        var query = @"
        SELECT 
            pe.idPedido AS IdPedido,
            pe.Name AS NombrePedido,
            pe.Volumen, 
            pe.Peso, 
            pe.EstadoPedido AS Estado,
            pe.FechaDespacho,
            emd.idEmpresa AS IdEmpresa,
            emd.Nombre AS NombreEmpresa
        FROM Empresa em
        JOIN Administrador admi USING (idEmpresa)
        JOIN Pedido pe ON pe.idAdministrador = admi.idAdministrador
        JOIN Empresa emd ON emd.idEmpresa = pe.idEmpresa
        WHERE em.idEmpresa = @IdEmpresa OR pe.idEmpresa = @IdEmpresa;
        ";

        return await Conexion.QueryAsync<PedidoEmpresaDTO>(query, new { IdEmpresa = idEmpresa });
    }

    public async Task<IEnumerable<HistorialDTO>> ObtenerHistorialCompleto(int idEmpresa)
    {
        var query = @"
            SELECT 
            p.idPedido,
            p.Name AS NombrePedido,
            p.Volumen,
            p.Peso,
            p.EstadoPedido,
            p.FechaDespacho,
            v.Tipo AS TipoVehiculo,
            v.Matricula AS Matricula,
            c.Name AS Conductor,
            eo.Nombre AS EmpresaOrigen,
            r.Origen,
            r.Destino,
            ed.Nombre AS EmpresaDestino,
            h.EstadoAnterior,
            h.EstadoNuevo,
            h.FechaCambio
        FROM Pedido p
        INNER JOIN Empresa eo ON p.idEmpresa = eo.idEmpresa
        INNER JOIN Ruta r ON p.idRuta = r.idRuta
        INNER JOIN Vehiculo v ON p.idVehiculo = v.idVehiculo
        INNER JOIN Conductor_has_Vehiculo cv ON v.idVehiculo = cv.idVehiculo
        INNER JOIN Conductor c ON cv.idConductor = c.idConductor
        INNER JOIN HistorialPedido h ON p.idPedido = h.idPedido
        LEFT JOIN Empresa ed ON r.Destino = ed.Nombre
        WHERE eo.idEmpresa = @IdEmpresa
        OR ed.idEmpresa = @IdEmpresa;";

        var Resultado = await Conexion.QueryAsync<HistorialDTO>(query, new { IdEmpresa = idEmpresa });
        return Resultado;
    }
}
