using Castle.DynamicProxy;
using Temporal.Core.Conventions;
using Temporal.Core.Conventions.CachingConventions;

namespace Temporal.Core.Interceptors
{
    public interface ICacheInterceptor : IInterceptor
    {
        void AddConvention(ICacheConvention convention);
        void ClearConventions();
    }
}
