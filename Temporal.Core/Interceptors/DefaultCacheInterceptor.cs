using System.Linq;
using Castle.DynamicProxy;
using Temporal.Core.Conventions.Caching;
using Temporal.Core.Exceptions;
using Temporal.Core.Interpreters;

namespace Temporal.Core.Interceptors
{
    public class DefaultCacheInterceptor : ICacheInterceptor
    {
        private readonly IShouldCacheInterpreter _shouldCacheInterpreter;
        private readonly ICacheProvider _cacheProvider;
        
        public DefaultCacheInterceptor(ICacheProvider cacheProvider)
        {
            _shouldCacheInterpreter = new ShouldCacheInterpreter();
            _cacheProvider = cacheProvider;
        }

        public void Intercept(IInvocation invocation)
        {
            if(_shouldCacheInterpreter.UseCache(invocation))
                _cacheProvider.HandleDataRequest(invocation);
            else
            {
                _cacheProvider.HandleDataChange(invocation);
                invocation.Proceed();
            }
        }

        public void AddConvention(ICacheConvention convention)
        {
            if(_shouldCacheInterpreter.Conventions.Any(c=>c.GetType()==convention.GetType()))
                throw new ConventionAlreadyRegisteredException(convention.GetType());

            _shouldCacheInterpreter.Conventions.Add(convention);
        }

        public void ClearConventions()
        {
            _shouldCacheInterpreter.Conventions.Clear();
        }
    }

}
