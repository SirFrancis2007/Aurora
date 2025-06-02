namespace Aurora.Core.Interfaces;

public interface IRepoListado<T>
{
    Task<IEnumerable<T>> Obtener { get; }
}
