using System.Collections.Generic;
using Castle.DynamicProxy;
using Temporal.Core.Conventions;

namespace Temporal.Core.Attributes
{
    public interface ICacheAttributeInterpreter
    {
        bool UseCache(IInvocation invocation);

        List<ICacheConvention> Conventions { get; set; } 
    }
}
