using Castle.DynamicProxy;
using Temporal.Core.Conventions.Caching;

namespace Temporal.Core.Interceptors
{
    public interface ICacheInterceptor : IInterceptor
    {
        void AddConvention(ICacheConvention convention);
        void ClearConventions();
    }
}
