using System.Data;
using Microsoft.Extensions.Configuration;
using MySqlConnector;


namespace Aurora.Dapper.Test;

public abstract class TestBase
{
    protected readonly IDbConnection Conexion;
    protected FixtureAurora fix;
    public TestBase() : this ("MySQL") { }
    public TestBase(string nombreConexion)
    {
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("AppSetting.json", optional: true, reloadOnChange: true)
            .Build();
        string cadena = config.GetConnectionString(nombreConexion)!;
        Conexion = new MySqlConnection(cadena);
        fix = new();
    }
}