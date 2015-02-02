using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using Temporal.Core.Conventions;

namespace Temporal.Core.Attributes
{
    public class CacheAttributeInterpreter : ICacheAttributeInterpreter
    {
        private readonly Dictionary<MethodInfo, IEnumerable<CacheAttribute>> _cacheAttributes;

        public List<ICacheConvention> Conventions { get; set; }

        public CacheAttributeInterpreter()
        {
            Conventions = new List<ICacheConvention>();
            _cacheAttributes = new Dictionary<MethodInfo, IEnumerable<CacheAttribute>>();
            
        }

        public bool UseCache(IInvocation invocation)
        {
            var shouldCache = AttributesAllowCache(invocation);
            if (!shouldCache)
                return false;

            shouldCache = ConventionsAllowCache(Conventions, invocation);

            return shouldCache;
        }

        private bool ConventionsAllowCache(IEnumerable<ICacheConvention> cacheConventions, IInvocation invocation)
        {
            bool result = false;

            foreach (var cacheConvention in cacheConventions)
            {
                if (cacheConvention.ShouldCache(invocation.MethodInvocationTarget))
                    result = true;
            }
            return result;
        }

        private bool AttributesAllowCache(IInvocation invocation)
        {
            var attributes = GetCustomAttributes(invocation.MethodInvocationTarget);
            return !attributes.Any(ca => ca is DontCacheAttribute);
        }

        private IEnumerable<CacheAttribute> GetCustomAttributes(MethodInfo method)
        {
            if (_cacheAttributes.ContainsKey(method))
                return _cacheAttributes[method];


            var attributes = Attribute.GetCustomAttributes(method, typeof(CacheAttribute), true); //method.GetCustomAttributes(typeof(CacheAttribute), true);
            _cacheAttributes.Add(method, attributes as CacheAttribute[]);
            return attributes as CacheAttribute[];
        }
    }
}
