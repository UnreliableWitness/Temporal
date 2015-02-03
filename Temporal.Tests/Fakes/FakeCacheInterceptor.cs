using Castle.DynamicProxy;
using Temporal.Core.Conventions.Caching;
using Temporal.Core.Conventions.Invalidation;
using Temporal.Core.Interceptors;

namespace Temporal.Tests.Fakes
{
    public class FakeCacheInterceptor : ICacheInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            throw new FakeException();
        }

        public void AddConvention(ICacheConvention convention)
        {
            throw new System.NotImplementedException();
        }

        public void ClearConventions()
        {
            throw new System.NotImplementedException();
        }

        public InvalidationConfiguration InvalidationConfiguration { get; set; }
    }
}
