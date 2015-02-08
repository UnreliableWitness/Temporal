using Temporal.Core.Conventions.Caching;
using Temporal.Core.Conventions.Invalidation;

namespace Temporal.Core
{
    public interface IRepositoryDecorator
    {
        CachingConventionsFluentInterface CacheIf { get; }
        InvalidationConventionsFluentInterface InvalidateOn { get; }
        T Decorate<T>(T target) where T : class;
    }
}