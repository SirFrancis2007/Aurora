using System.Data;
using Aurora.Core;
using Aurora.Core.Interfaces;

namespace Aurora.Dapper.ADO;

public class RepoRuta : RepoGenerico, IRepoRuta
{
    public RepoRuta(IDbConnection conexion) : base(conexion)
    {
    }

    public void CrearRuta(Ruta NewRuta)
    {
        throw new NotImplementedException();
    }

    public void EliminarRura(int idRuta)
    {
        throw new NotImplementedException();
    }

    public List<Ruta> ListarRutas()
    {
        throw new NotImplementedException();
    }

    public void ObtenerRutaPorCondicion()
    {
        throw new NotImplementedException();
    }

    public Ruta ObtenerRutaPorId(int idRuta)
    {
        throw new NotImplementedException();
    }
}
