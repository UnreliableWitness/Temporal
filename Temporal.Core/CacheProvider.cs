using System;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace Temporal.Core
{
    public class CacheProvider : ICacheProvider
    {
        private readonly ICacheContainer _cacheContainer;

        public CacheProvider(ICacheContainer cacheContainer)
        {
            _cacheContainer = cacheContainer;
        }

        public CacheProvider() : this(new CacheContainer())
        {

        }

        public void Handle(IInvocation invocation)
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
    }
}
