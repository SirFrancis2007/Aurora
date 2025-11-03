using Aurora.Core.Interfaces;

namespace Aurora.Dapper.ADO;

public class RepoAutenticar
{
    private readonly IEnumerable<IRepoAutenticacion> _repos;

        public RepoAutenticar(IEnumerable<IRepoAutenticacion> repos)
        {
            _repos = repos;
        }

        public IRepoAutenticacion? ObtenerPorRol(string rol)
        {
            return _repos.FirstOrDefault(r =>r.Rol.Equals(rol, StringComparison.OrdinalIgnoreCase));
        }
}
