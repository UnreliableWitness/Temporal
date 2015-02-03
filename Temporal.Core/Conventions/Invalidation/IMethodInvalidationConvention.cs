using System.Reflection;

namespace Temporal.Core.Conventions.Invalidation
{
    public interface IMethodInvalidationConvention
    {
        bool ShouldInvalidate(MethodInfo methodInfo);
    }
}
