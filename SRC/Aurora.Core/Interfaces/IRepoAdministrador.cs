namespace Aurora.Core.Interfaces;

public interface IRepoAdministrador : IRepoAlta<Administrador>, IRepoDetalle<Administrador, int>, IRepoListado<Administrador> 
{
    //public void Loguearse (string Nombre, string password);
    //public void CerrarSesion();
    public void CrearPedido(Pedido xNewPedido);
    public void ObtenerPedidoXAdmin(int idadministrador);
}
