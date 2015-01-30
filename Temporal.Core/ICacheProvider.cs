using Castle.DynamicProxy;

namespace Temporal.Core
{
    public interface ICacheProvider
    {
        void Handle(IInvocation invocation);
    }
}
