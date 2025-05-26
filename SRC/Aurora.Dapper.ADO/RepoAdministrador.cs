using System.Data;
using Aurora.Core;
using Aurora.Core.Interfaces;
using Dapper;
#pragma warning restore format
namespace Aurora.Dapper.ADO;

public class RepoAdministrador : RepoGenerico, IRepoAdministrador
{
    public RepoAdministrador(IDbConnection conexion) : base(conexion)
    {
    }

    public void Alta(Administrador NewAdmin)
    {
        var parametros = new DynamicParameters();
        parametros.Add("xName",NewAdmin.Nombre);
        parametros.Add("xPassword", NewAdmin.Password);
        parametros.Add("xEmpresa_idEmpresa", NewAdmin.IdEmpresa);

        try
        {
            Conexion.Execute("SPNuevoAdministrador", parametros);
        }
        catch (System.Exception)
        {
            throw new Exception("Error al agregar un nuevo administrador");
        }
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

    public Administrador? Detalle(int xidAdmin)
    {
        var Query = @"SELECT * FROM Administrador where idAdministrador = {xidAdmin}";
        var repuesta = Conexion.QueryFirstOrDefault<Administrador>(Query);
        return repuesta;
    }


    public IEnumerable<Administrador> Obtener()
    {
        var Query = @"SELECT * FROM Administrador";
        var repuesta = Conexion.Query<Administrador>(Query);
        return repuesta;
    }

    public void ObtenerPedidoXAdmin(int idadministrador)
    {
        var Query = @"SELECT 
                        a.Name AS NombreAdministrador,
                        p.idPedido,
                        p.Name AS NombrePedido,
                        p.Volumen,
                        p.Peso,
                        p.EstadoPedido,
                        p.FechaDespacho,
                        p.EmpresaDestino,
                        p.Ruta_idRuta
                        FROM Administrador a
                        JOIN Pedido p USING (idAdministrador)
                        WHERE a.idAdministrador = @idadministrador;";
        var repuesta = Conexion.QueryFirstOrDefault<Administrador>(Query);
    }
}
