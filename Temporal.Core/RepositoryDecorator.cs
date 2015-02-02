using System.Collections.Generic;
using Castle.DynamicProxy;
using Temporal.Core.Conventions;
using Temporal.Core.Interceptors;

namespace Temporal.Core
{
    public sealed class RepositoryDecorator
    {
        private readonly ProxyGenerator _proxyGenerator;
        public ICacheInterceptor CacheInterceptor { get; set; }

        private readonly ConventionsFluentInterface _conventions;
        
        public ConventionsFluentInterface Conventions { get { return _conventions; } }

        public RepositoryDecorator()
        {
            var cacheContainer = new CacheContainer();
            var cacheProvider = new CacheProvider(cacheContainer);
            CacheInterceptor = new DefaultCacheInterceptor(cacheProvider);

            _proxyGenerator = new ProxyGenerator();
            _conventions = new ConventionsFluentInterface(this);
        }

        public RepositoryDecorator(ICacheInterceptor cacheInterceptor)
        {
            _proxyGenerator = new ProxyGenerator();
            CacheInterceptor = cacheInterceptor;
            
        }

        public T Decorate<T>(T target) where T : class
        {
            var decoratedRepo = _proxyGenerator.CreateInterfaceProxyWithTarget(target, CacheInterceptor);
            return decoratedRepo;
        }

        
    }


}
