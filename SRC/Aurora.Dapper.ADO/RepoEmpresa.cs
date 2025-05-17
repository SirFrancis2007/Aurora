using System.Data;
using Aurora.Core;
using Aurora.Core.Interfaces;
using Dapper;

namespace Aurora.Dapper.ADO;

public class RepoEmpresa : RepoGenerico, IRepoEmpresa
{
    public RepoEmpresa(IDbConnection conexion) : base(conexion)
    {
    }

    // Alta de empresa
    public void Alta(Empresa NuevaEmpresa)
    {
        var parametros = new DynamicParameters();
        parametros.Add("xNombre", NuevaEmpresa.Nombre);
        try
        {
            Conexion.Execute("PSCrearEmpresa", parametros); // "PSCrearEmpresa" SP para crear empresa
        }
        catch (System.Exception)
        {
            throw new Exception("Esta empresa ya se encuentra Registrada");
        }
    }
    public void AgregarAdministrador(Administrador xAdministrador)
    {
        var parametros = new DynamicParameters();
        parametros.Add("xidAdministrador", xAdministrador.IdAdministrador);
        parametros.Add("xNombre", xAdministrador.Nombre);
        parametros.Add("xpassword", xAdministrador.Password);
        parametros.Add("xidEmpresa", xAdministrador.IdEmpresa);

        try
        {
            Conexion.Execute("SPNuevoAdministrador", parametros); // "SPNuevoAdministrador" SP para añadir Administrador
        }
        catch (System.Exception)
        {
            throw new ConstraintException($"El admnistrado ya se encuentra registrado");
        }
    }

    public Empresa? Detalle(uint indiceABuscar)
    {
        var query = @"Select * From Empresa where idEmpresa = {indiceABuscar}";
        var Resultado = Conexion.QueryFirstOrDefault<Empresa>(query);
        return Resultado;
    }

    public void EliminarAdministrador(int xidadministrador)
    {
        var parametros = new DynamicParameters();
        parametros.Add("xidAdministrador", xidadministrador);

        try
        {
            Conexion.Execute("SPDelAdministrador", parametros);
        }
        catch (System.Exception)
        {
            throw new Exception("¡Error al eliminar al administrador!");
        }
    }

    public void EliminarEmpresa(int idempresa)
    {
        var parametros = new DynamicParameters();
        parametros.Add("@xidEmpresa", idempresa);

        try
        {
            Conexion.Execute("SPDelEmpresa", parametros);
        }
        catch (System.Exception)
        {
            throw new Exception("¡Error al eliminar al administrador!");
        }
    }

    public IEnumerable<Empresa> Obtener()
    {
        var query = @"Select * From Empresa";
        var Resultado = Conexion.Query<Empresa>(query);
        return Resultado;
    }

    public IEnumerable<Pedido> ObtenerPedidos(int xidEmpresa)
    {
        var query = @"Select * 
                    From Pedido 
                    Where p.EmpresaDestino = {xidEmpresa} OR a.Empresa_idEmpresa = {xidEmpresa} 
                    ORDER BY p.FechaDespacho DESC";
        var resultados = Conexion.Query<Pedido>(query);
        return resultados;
    }
}
