using System.Reflection;

namespace Temporal.Core.Conventions.Caching
{
    public interface ICacheConvention
    {
        bool ShouldCache(MethodInfo methodInfo);
    }
}
