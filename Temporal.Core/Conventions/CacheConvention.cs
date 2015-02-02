using System.Reflection;

namespace Temporal.Core.Conventions
{
    public interface ICacheConvention
    {
        bool ShouldCache(MethodInfo methodInfo);
    }
}
