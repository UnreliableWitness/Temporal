using System.Reflection;

namespace Temporal.Core.Conventions
{
    public class DefaultConvention : ICacheConvention
    {

        public bool ShouldCache(MethodInfo methodInfo)
        {
            var methodName = methodInfo.Name;
            return methodName.StartsWith("Retrieve") || methodName.StartsWith("Get") || methodName.StartsWith("Fetch");
        }
    }
}
