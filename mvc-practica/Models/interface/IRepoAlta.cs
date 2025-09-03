namespace Aurora.Core.Interfaces;

public interface IRepoAlta<T>
{
    Task AltaAsync(T elemento);
}
