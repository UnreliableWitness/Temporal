using System.Linq;
using Castle.DynamicProxy;
using Temporal.Core.Conventions;
using Temporal.Core.Conventions.CachingConventions;
using Temporal.Core.Exceptions;

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
            if(_cacheAttributeInterpreter.Conventions.Any(c=>c.GetType()==convention.GetType()))
                throw new ConventionAlreadyRegisteredException(convention.GetType());

            _cacheAttributeInterpreter.Conventions.Add(convention);
        }

        public void ClearConventions()
        {
            _cacheAttributeInterpreter.Conventions.Clear();
        }
    }

}
