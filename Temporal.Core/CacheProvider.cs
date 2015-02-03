using System;
using System.Linq;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Temporal.Core.Conventions.Invalidation;

namespace Temporal.Core
{
    public class CacheProvider : ICacheProvider
    {
        private readonly ICacheContainer _cacheContainer;

        public InvalidationConfiguration InvalidationConfiguration { get; set; }

        public CacheProvider(ICacheContainer cacheContainer)
        {
            _cacheContainer = cacheContainer;
        }

        public CacheProvider() : this(new CacheContainer())
        {

        }

        public void HandleDataRequest(IInvocation invocation)
        {
            var returnType = invocation.Method.ReturnType;
            if (returnType != typeof (void))
            {
                string cacheKey;
                if (_cacheContainer != null && _cacheContainer.CacheKeyGenerator.TryBuildCacheKey(invocation.Method, invocation.Arguments,
                    out cacheKey))
                {
                    object result;
                    if (_cacheContainer.TryGet(cacheKey, out result))
                    {
                        invocation.ReturnValue = result;
                    }
                    else
                    {
                        invocation.Proceed();
                        object returnValue = invocation.ReturnValue;
                        if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof (Task<>))
                        {
                            var task = (Task) returnValue;
                            task.ContinueWith(_ =>
                            {
                                _cacheContainer.TryAdd(cacheKey, returnValue, TimeSpan.FromMinutes(10));
                            });
                        }
                        else
                        {
                            _cacheContainer.TryAdd(cacheKey, returnValue, TimeSpan.FromMinutes(10));
                        }
                    }
                }
            }
        }

        public void HandleDataChange(IInvocation invocation)
        {
            if (InvalidationConfiguration.MethodInvalidation &&
                InvalidationConfiguration.MethodInvalidationConventions.Any())
            {
                foreach (var methodInvalidationConvention in InvalidationConfiguration.MethodInvalidationConventions)
                {
                    if (methodInvalidationConvention.ShouldInvalidate(invocation.MethodInvocationTarget))
                    {
                        _cacheContainer.InvalidateAll();
                        return;
                    }
                }
            }
        }
    }
}
