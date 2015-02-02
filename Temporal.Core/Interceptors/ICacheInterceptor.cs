using Castle.DynamicProxy;
using Temporal.Core.Conventions;

namespace Temporal.Core.Interceptors
{
    public interface ICacheInterceptor : IInterceptor
    {
        void AddConvention(ICacheConvention convention);
        void ClearConventions();
    }
}
