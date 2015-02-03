using System.Collections.Generic;

namespace Temporal.Core.Conventions.Invalidation
{
    public class InvalidationConfiguration
    {
        public bool MethodInvalidation { get; set; }

        public List<IMethodInvalidationConvention> MethodInvalidationConventions { get; set; }

        public InvalidationConfiguration()
        {
            MethodInvalidationConventions = new List<IMethodInvalidationConvention>();
        }
    }
}
