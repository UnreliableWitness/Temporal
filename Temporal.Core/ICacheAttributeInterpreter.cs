using System.Collections.Generic;
using Castle.DynamicProxy;
using Temporal.Core.Conventions;
using Temporal.Core.Conventions.CachingConventions;

namespace Temporal.Core
{
    public interface ICacheAttributeInterpreter
    {
        bool UseCache(IInvocation invocation);

        List<ICacheConvention> Conventions { get; set; } 
    }
}
