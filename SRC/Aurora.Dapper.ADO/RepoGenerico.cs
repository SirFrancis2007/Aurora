using System.Data;

namespace Aurora.Dapper.ADO;

public abstract class RepoGenerico
{
    protected readonly IDbConnection Conexion;
    public RepoGenerico(IDbConnection conexion) => Conexion = conexion;
}
