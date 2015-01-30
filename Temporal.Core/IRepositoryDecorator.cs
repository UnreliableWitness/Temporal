namespace Temporal.Core
{
    public interface IRepositoryDecorator
    {
        T Decorate<T>() where T : class;
    }
}