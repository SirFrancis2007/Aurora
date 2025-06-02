namespace Aurora.Core.Interfaces;

public interface IRepoAlta<T>
{
    Task Alta(T elemento);
}
