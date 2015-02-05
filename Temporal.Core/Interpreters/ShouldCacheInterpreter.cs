using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using Temporal.Core.Attributes;
using Temporal.Core.Conventions.Caching;

namespace Temporal.Core.Interpreters
{
    public class ShouldCacheInterpreter : IShouldCacheInterpreter
    {
        private readonly ConcurrentDictionary<MethodInfo, IEnumerable<CacheAttribute>> _cacheAttributes;

        public List<ICacheConvention> Conventions { get; set; }

        public ShouldCacheInterpreter()
        {
            Conventions = new List<ICacheConvention>();
            _cacheAttributes = new ConcurrentDictionary<MethodInfo, IEnumerable<CacheAttribute>>();
        }

        public bool UseCache(IInvocation invocation)
        {
            if (!HasReturnValue(invocation))
                return false;

            var shouldCache = AttributesAllowCache(invocation);
            if (!shouldCache)
                return false;

            shouldCache = ConventionsAllowCache(Conventions, invocation);

            return shouldCache;
        }

        private bool HasReturnValue(IInvocation invocation)
        {
            return (invocation.MethodInvocationTarget.ReturnType != typeof (void));
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


            var attributes = Attribute.GetCustomAttributes(method, typeof(CacheAttribute), true) as CacheAttribute[]; //method.GetCustomAttributes(typeof(CacheAttribute), true);
            _cacheAttributes.AddOrUpdate(method, attributes, (info, enumerable) =>
            {
                enumerable = attributes;
                return enumerable;
            });
            return attributes;
        }
    }
}
