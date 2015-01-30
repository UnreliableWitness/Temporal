using Castle.DynamicProxy;

namespace Temporal.Core.Interceptors
{
    public class DefaultCacheInterceptor : ICacheInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed();
        }
    }
}
