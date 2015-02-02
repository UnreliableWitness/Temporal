using System;

namespace Temporal.Core.Attributes.Invalidation
{
    [AttributeUsage(AttributeTargets.Method)]
    public class InvalidationSourceAttribute : InvalidationAttribute
    {
        
    }

    public enum InvalidationMethod
    {
        ByType,
        ByNamingConvention

    }
}
