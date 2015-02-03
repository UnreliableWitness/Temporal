using System.Collections.Generic;
using Castle.DynamicProxy;
using Temporal.Core.Conventions.Caching;

namespace Temporal.Core.Interpreters
{
    public interface IShouldCacheInterpreter
    {
        bool UseCache(IInvocation invocation);

        List<ICacheConvention> Conventions { get; set; } 
    }
}
