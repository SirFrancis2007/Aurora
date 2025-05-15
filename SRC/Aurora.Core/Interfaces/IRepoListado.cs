namespace Aurora.Core.Interfaces;

public interface IRepoListado<T>
{
    IEnumerable<T> Obtener();
}
