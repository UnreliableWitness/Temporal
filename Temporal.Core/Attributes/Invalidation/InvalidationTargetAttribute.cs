using System;

namespace Temporal.Core.Attributes.Invalidation
{
    [AttributeUsage(AttributeTargets.Method)]
    public class InvalidationTargetAttribute : InvalidationAttribute
    {
    }
}
