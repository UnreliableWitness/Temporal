using Castle.DynamicProxy;
using Temporal.Core.Attributes;
using Temporal.Core.Conventions;

namespace Temporal.Core.Interceptors
{
    public class DefaultCacheInterceptor : ICacheInterceptor
    {
        private readonly ICacheAttributeInterpreter _cacheAttributeInterpreter;
        private readonly ICacheProvider _cacheProvider;
        
        public DefaultCacheInterceptor(ICacheProvider cacheProvider)
        {
            _cacheAttributeInterpreter = new CacheAttributeInterpreter();
            _cacheProvider = cacheProvider;
        }

        public void Intercept(IInvocation invocation)
        {
            if(_cacheAttributeInterpreter.UseCache(invocation))
                _cacheProvider.Handle(invocation);
            else
            {
                invocation.Proceed();
            }
        }

        public void AddConvention(ICacheConvention convention)
        {
            _cacheAttributeInterpreter.Conventions.Add(convention);
        }

        public void ClearConventions()
        {
            _cacheAttributeInterpreter.Conventions.Clear();
        }
    }
}
