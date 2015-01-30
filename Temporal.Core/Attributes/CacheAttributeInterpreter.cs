using System.Collections.Generic;
using System.Linq;

namespace Temporal.Core.Attributes
{
    public class CacheAttributeInterpreter : ICacheAttributeInterpreter
    {
        public bool UseCache(IEnumerable<CacheAttribute> cacheAttributes)
        {
            //do some caching here as well
            //basic implementation, ability to exend with other attributes in the future
            return !cacheAttributes.Any(ca => ca is DontCacheAttribute);
        }
    }
}
