using Castle.DynamicProxy;
using Temporal.Core;
using Temporal.Core.Interceptors;

namespace Temporal.Tests.Fakes
{
    public class FakeCacheInterceptor : ICacheInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            throw new FakeException();
        }
    }
}
