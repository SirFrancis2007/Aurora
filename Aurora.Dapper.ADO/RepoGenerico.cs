using System.Data;

namespace Aurora.Dapper.ADO;

public class RepoGenerico
{
    protected readonly IDbConnection Conexion;
    public RepoGenerico(IDbConnection conexion) => Conexion = conexion;
}
