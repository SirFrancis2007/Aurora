using System.Data;
using Aurora.Core;
using Aurora.Core.Interfaces;
using Dapper;

namespace Aurora.Dapper.ADO;

public class RepoEmpresa : RepoGenerico, IRepoEmpresa
{
    private uint _idempresanueva;
    Task<IEnumerable<Empresa>> IRepoListado<Empresa>.ObtenerAsync => Obtener();

    public RepoEmpresa(IDbConnection conexion) : base(conexion)
    {
    }

    public async Task<IEnumerable<Empresa>> Obtener()
    {
        var query = @"Select * From Empresa";
        var Resultado = await Conexion.QueryAsync<Empresa>(query);
        return Resultado;
    }

    public async Task AltaAsync(Empresa NuevaEmpresa)
    {
        var parametros = new DynamicParameters();
        parametros.Add("xidEmpresa", direction: ParameterDirection.Output);
        parametros.Add("xNombre", NuevaEmpresa.Nombre);
        try
        {
            await Conexion.ExecuteAsync("PSCrearEmpresa", parametros, commandType: CommandType.StoredProcedure); // "PSCrearEmpresa" SP para crear empresa
            _idempresanueva = (uint)parametros.Get<int>("xidEmpresa");
        }
        catch (System.Exception ex)
        {
            throw new Exception($"Error al crear empresa: {ex.Message}", ex);
        }
    } //Check Funcionando 24/06

    public async Task<Empresa>? DetalleAsync(uint indiceABuscar)
    {
        var query = @"Select * From Empresa where idEmpresa = @IndiceEmpresa;";
        var Resultado = await Conexion.QueryFirstOrDefaultAsync<Empresa>(query, new { IndiceEmpresa = indiceABuscar });
        return Resultado;
    } //Check Funcionando 24/06

    public async Task EliminarAdministradorAsync(int xidadministrador)
    {
        var parametros = new DynamicParameters();
        parametros.Add("xidAdministrador", xidadministrador);
        try
        {
            await Conexion.ExecuteAsync("SPDelAdministrador", parametros);
        }
        catch (System.Exception)
        {
            throw new Exception("¡Error al eliminar al administrador!");
        }
    } //Check Funcionando 24/06

    public async Task EliminarEmpresaAsync(int idempresa)
    {
        var parametros = new DynamicParameters();
        parametros.Add("@xidEmpresa", idempresa);
        try
        {
            await Conexion.ExecuteAsync("SPDelEmpresa", parametros);
        }
        catch (System.Exception)
        {
            throw new Exception("¡Error al eliminar al administrador!");
        }
    } //Check Funcionando 24/06

    public Task<Empresa?> ObtenerPorNombreAsync(string Nombre)
    {
        var Query = "Select idEmpresa From Empresa where Nombre = @InNombreEmpresa;";
        var resultado = Conexion.QueryFirstOrDefaultAsync<Empresa>(Query, new { InNombreEmpresa = Nombre });
        return resultado;
    } //Check Funcionando 24/06

    public async Task<IEnumerable<PedidoEmpresaDTO>> ObtenerPedidosAsync(int _idempresanueva)
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

        return await Conexion.QueryAsync<PedidoEmpresaDTO>(query, new { IdEmpresa = _idempresanueva });
    }

    public async Task<bool> LoginAsync(string Nombre)
    {
        var funtionLogin = "SELECT FLoginEmpresa(@xNombre);";
        var result = await Conexion.ExecuteScalarAsync<bool>(funtionLogin, new { xNombre = Nombre });
        return result;
    }

    public async Task<IEnumerable<Administrador>> ObtenerAdministradoresXempresaAsync(int xidEmpresa)
    {
        var query = @"Select * From Administrador where idEmpresa = @IdEmpresa;";
        var Resultado = await Conexion.QueryAsync<Administrador>(query, new { IdEmpresa = xidEmpresa });
        return Resultado;
    }

    public async Task<IEnumerable<Conductor>> ObtenerConductoresXempresaAsync(int xidEmpresa)
    {
        var query = @"Select * From Conductor where idEmpresa = @IdEmpresa;";
        var Resultado = await Conexion.QueryAsync<Conductor>(query, new { IdEmpresa = xidEmpresa });
        return Resultado;
    }

    public async Task<IEnumerable<Vehiculo>> ObtenerVehiculosXempresaAsync(int xidEmpresa)
    {
        var query = @"Select * From Vehiculo where idEmpresa = @IdEmpresa;";
        var Resultado = await Conexion.QueryAsync<Vehiculo>(query, new { IdEmpresa = xidEmpresa });
        return Resultado;
    }

    public Task<IEnumerable<HistorialDTO>> ObtenerHistorialXempresaAsync(int xidEmpresa)
    {
        var query = @"
            SELECT 
            p.idPedido,
            p.Name AS NombrePedido,
            p.Volumen,
            p.Peso,
            p.EstadoPedido,
            p.FechaDespacho,
            v.Tipo AS VehiculoTipo,
            v.Matricula AS VehiculoMatricula,
            c.Name AS NombreConductor,
            eo.Nombre AS EmpresaOrigen,
            p.idEmpresa AS idEmpresaOrigen,
            r.Origen AS RutaOrigen,
            r.Destino AS RutaDestino,
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

        var Resultado = Conexion.QueryAsync<HistorialDTO>(query, new { IdEmpresa = xidEmpresa });
        return Resultado;
    }
}
