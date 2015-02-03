using System.Reflection;

namespace Temporal.Core.Conventions.Invalidation
{
    public class DefaultMethodInvalidationConvention : IMethodInvalidationConvention
    {
        public bool ShouldInvalidate(MethodInfo methodInfo)
        {
            var name = methodInfo.Name.ToLower();

            return name.StartsWith("update") || name.StartsWith("create") || name.StartsWith("set") ||
                   name.StartsWith("save");
        }
    }
}
