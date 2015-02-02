using System.Reflection;

namespace Temporal.Core.Conventions.CachingConventions
{
    public interface ICacheConvention
    {
        bool ShouldCache(MethodInfo methodInfo);
    }
}
