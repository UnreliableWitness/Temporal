using System;
using System.Collections.Generic;
using System.Reflection;
using Castle.DynamicProxy;
using Temporal.Core.Attributes;

namespace Temporal.Core.Interceptors
{
    public class DefaultCacheInterceptor : ICacheInterceptor
    {
        private readonly Dictionary<MethodInfo, IEnumerable<CacheAttribute>> _cacheAttributes;
        private readonly ICacheAttributeInterpreter _cacheAttributeInterpreter;
        private readonly ICacheProvider _cacheProvider;

        public DefaultCacheInterceptor(ICacheProvider cacheProvider)
        {
            _cacheAttributes = new Dictionary<MethodInfo, IEnumerable<CacheAttribute>>();
            _cacheAttributeInterpreter = new CacheAttributeInterpreter();
            _cacheProvider = cacheProvider;
        }

        public void Intercept(IInvocation invocation)
        {
            var attributes = GetCustomAttributes(invocation.MethodInvocationTarget);

            if(_cacheAttributeInterpreter.UseCache(attributes))
                _cacheProvider.Handle(invocation);
            else
            {
                invocation.Proceed();
            }
        }

        private IEnumerable<CacheAttribute> GetCustomAttributes(MethodInfo method)
        {
            if (_cacheAttributes.ContainsKey(method))
                return _cacheAttributes[method];

            var attributes = Attribute.GetCustomAttributes(method, typeof (CacheAttribute), true); //method.GetCustomAttributes(typeof(CacheAttribute), true);
            _cacheAttributes.Add(method, attributes as CacheAttribute[]);
            return attributes as CacheAttribute[];
        }
    }
}
