using System.Reflection;

namespace Temporal.Core.Conventions.Caching
{
    public class DefaultCachingConvention : ICacheConvention
    {

        public bool ShouldCache(MethodInfo methodInfo)
        {
            var methodName = methodInfo.Name;
            return methodName.StartsWith("Retrieve") || methodName.StartsWith("Get") || methodName.StartsWith("Fetch");
        }
    }
}
