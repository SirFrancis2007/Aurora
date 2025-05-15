using System.Data;
using Aurora.Core;
using Aurora.Core.Interfaces;
#pragma warning restore format
namespace Aurora.Dapper.ADO;

public class RepoAdministrador : RepoGenerico, IRepoAdministrador
{
    public RepoAdministrador(IDbConnection conexion) : base(conexion)
    {
    }

    public void Alta(Administrador elemento)
    {
        throw new NotImplementedException();
    }

    /*public void CerrarSesion()
    {
        throw new NotImplementedException();
    }*/

    public void CrearPedido(Pedido xNewPedido)
    {
        throw new NotImplementedException();
    }

    public Administrador? Detalle(int indiceABuscar)
    {
        throw new NotImplementedException();
    }

    /*public void Loguearse(string Nombre, string password)
    {
        throw new NotImplementedException();
    }*/

    public IEnumerable<Administrador> Obtener()
    {
        throw new NotImplementedException();
    }

    public void ObtenerPedidoXAdmin(int idadministrador)
    {
        throw new NotImplementedException();
    }
}
