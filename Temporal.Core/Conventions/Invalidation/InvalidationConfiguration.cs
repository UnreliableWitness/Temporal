using System.Collections.Generic;
using System.Runtime.Caching;

namespace Temporal.Core.Conventions.Invalidation
{
    public class InvalidationConfiguration
    {
        public bool MethodInvalidation { get; set; }

        public List<IMethodInvalidationConvention> MethodInvalidationConventions { get; set; }

        public CacheItemPolicy CacheItemPolicy { get; set; }

        public InvalidationConfiguration(CacheItemPolicy cacheItemPolicy)
        {
            CacheItemPolicy = cacheItemPolicy;
            MethodInvalidationConventions = new List<IMethodInvalidationConvention>();
        }
    }
}
