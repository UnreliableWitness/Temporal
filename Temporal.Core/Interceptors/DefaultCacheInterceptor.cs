using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using Temporal.Core.Attributes;

namespace Temporal.Core.Interceptors
{
    public class DefaultCacheInterceptor : ICacheInterceptor
    {
        private readonly Dictionary<MethodInfo, IEnumerable<CacheAttribute>> _cacheAttributes;
        private readonly ICacheAttributeInterpreter _cacheAttributeInterpreter;

        public DefaultCacheInterceptor()
        {
            _cacheAttributes = new Dictionary<MethodInfo, IEnumerable<CacheAttribute>>();
            _cacheAttributeInterpreter = new CacheAttributeInterpreter();
        }

        public void Intercept(IInvocation invocation)
        {
            var attributes = GetCustomAttributes(invocation.MethodInvocationTarget);

            if(_cacheAttributeInterpreter.UseCache(attributes))
                invocation.Proceed();
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
