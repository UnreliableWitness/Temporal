using System.Collections.Generic;

namespace Temporal.Core.Attributes
{
    public interface ICacheAttributeInterpreter
    {
        bool UseCache(IEnumerable<CacheAttribute> cacheAttributes);
    }
}
