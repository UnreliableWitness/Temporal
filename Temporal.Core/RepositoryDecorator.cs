using System.Collections.ObjectModel;
using Castle.DynamicProxy;
using Temporal.Core.Conventions.Caching;
using Temporal.Core.Conventions.Invalidation;
using Temporal.Core.Interceptors;

namespace Temporal.Core
{
    public sealed class RepositoryDecorator
    {
        private readonly ProxyGenerator _proxyGenerator;
        internal ICacheInterceptor CacheInterceptor { get; set; }

        private readonly CachingConventionsFluentInterface _cachingConventions;
        public CachingConventionsFluentInterface CacheIf { get { return _cachingConventions; } }
        internal CacheProvider CacheProvider; 

        private readonly InvalidationConventionsFluentInterface _invalidationConventions;
        public InvalidationConventionsFluentInterface InvalidateOn { get { return _invalidationConventions; } }

        public RepositoryDecorator()
        {
            var cacheContainer = new CacheContainer();
            CacheProvider = new CacheProvider(cacheContainer);
            CacheInterceptor = new DefaultCacheInterceptor(CacheProvider);

            _proxyGenerator = new ProxyGenerator();
            _cachingConventions = new CachingConventionsFluentInterface(this);
            _invalidationConventions = new InvalidationConventionsFluentInterface(this);
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
