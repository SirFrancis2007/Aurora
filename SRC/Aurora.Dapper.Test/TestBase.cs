using System.Data;

namespace Aurora.Dapper.Test;

public abstract class TestBase
{
    protected readonly IDbConnection Conexion;
    public TestBase() : this ("MySQL") { }
    public TestBase(string nombreConexion)
    {
        // IConfiguration config = new ConfigurationBuilder()
        //     .AddJsonFile("AppSetting.json", optional: true, reloadOnChange: true)
        //     .Build();
        // string cadena = config.GetConnectionString(nombreConexion)!;
        // Conexion = new MySqlConnection(cadena);
    }
}