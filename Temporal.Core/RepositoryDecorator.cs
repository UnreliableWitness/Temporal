using Castle.DynamicProxy;
using Temporal.Core.Interceptors;

namespace Temporal.Core
{
    public sealed class RepositoryDecorator
    {
        private readonly ProxyGenerator _proxyGenerator;
        private readonly ICacheInterceptor _cacheInterceptor;

        public RepositoryDecorator() : this(new DefaultCacheInterceptor())
        {
            _proxyGenerator = new ProxyGenerator();
        }

        public RepositoryDecorator(ICacheInterceptor cacheInterceptor)
        {
            _proxyGenerator = new ProxyGenerator();
            _cacheInterceptor = cacheInterceptor;
        }

        public T Decorate<T>(T target) where T : class
        {
            var decoratedRepo = _proxyGenerator.CreateInterfaceProxyWithTarget<T>(target, _cacheInterceptor);
            return decoratedRepo;
        }
    }
}
