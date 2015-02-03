using Castle.DynamicProxy;

namespace Temporal.Core
{
    public interface ICacheProvider
    {
        void HandleDataRequest(IInvocation invocation);
        void HandleDataChange(IInvocation invocation);
    }
}
