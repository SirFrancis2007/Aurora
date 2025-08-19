using System.Data;
using Aurora.Core;
using Aurora.Core.Interfaces;
using Dapper;

namespace Aurora.Dapper.ADO;

public class RepoEmpresa : RepoGenerico, IRepoEmpresa
{
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
        parametros.Add("xidEmpresa", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parametros.Add("xNombre", NuevaEmpresa.Nombre);
        try
        {
            await Conexion.ExecuteAsync("PSCrearEmpresa", parametros, commandType: CommandType.StoredProcedure); // "PSCrearEmpresa" SP para crear empresa
            NuevaEmpresa.IdEmpresa = (uint)parametros.Get<int>("xidEmpresa");
        }
        catch (System.Exception)
        {
            throw new Exception("Esta empresa ya se encuentra Registrada");
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

    public async Task<IEnumerable<(Pedido, Empresa)>> ObtenerPedidosAsync(int xidEmpresa)
    {
        var query = @"SELECT 
            pe.idPedido,
            pe.Name AS Nombre_Pedido,
            pe.Volumen, 
            pe.Peso, 
            pe.EstadoPedido, 
            pe.FechaDespacho, 
            em.idEmpresa AS idEmpresaOrigen,
            em.Nombre AS Nombre_Empresa_Origen, 
            emd.idEmpresa AS idEmpresa,
            emd.Nombre AS Nombre_Empresa_Destino,
            admi.idAdministrador,
            admi.Nombre AS Nombre_Administrador
            FROM Empresa em
            JOIN Administrador admi USING (idEmpresa)
            JOIN Pedido pe ON pe.idAdministrador = admi.idAdministrador
            JOIN Empresa emd ON emd.idEmpresa = pe.idEmpresa
            WHERE em.idEmpresa = @IdEmpresa OR pe.idEmpresa = @IdEmpresa;
            ";

        var resultados = await Conexion.QueryAsync<Pedido, Empresa, (Pedido, Empresa)>(
            query,
            (pedido, empresa) => (pedido, empresa),
            new { IdEmpresa = xidEmpresa },
            splitOn: "idEmpresa" // Indica dónde empieza el mapeo para Empresa
        );

    return resultados;
    } //Check Funcionando 24/06

    public Task<Empresa?> ObtenerPorNombreAsync(string Nombre)
    {
        var Query = "Select Nombre From Empresa where Nombre = @InNombreEmpresa;";
        var resultado = Conexion.QueryFirstOrDefaultAsync<Empresa>(Query, new {InNombreEmpresa = Nombre}); 
        return resultado;   
    } //Check Funcionando 24/06
}
