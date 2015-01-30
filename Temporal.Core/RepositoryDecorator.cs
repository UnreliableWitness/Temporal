using System.Collections.Generic;
using Castle.DynamicProxy;
using Temporal.Core.Conventions;
using Temporal.Core.Interceptors;

namespace Temporal.Core
{
    public sealed class RepositoryDecorator
    {
        private readonly ProxyGenerator _proxyGenerator;
        private readonly ICacheInterceptor _cacheInterceptor;

        private readonly ConventionsFluentInterface _conventions;

        internal List<ICacheConvention> RegisteredConventions;

        public ConventionsFluentInterface Conventions { get { return _conventions; } }

        public RepositoryDecorator() : this(new DefaultCacheInterceptor(new CacheProvider()))
        {
            _proxyGenerator = new ProxyGenerator();
            _conventions = new ConventionsFluentInterface(this);
        }

        public RepositoryDecorator(ICacheInterceptor cacheInterceptor)
        {
            _proxyGenerator = new ProxyGenerator();
            _cacheInterceptor = cacheInterceptor;
        }

        public T Decorate<T>(T target) where T : class
        {
            var decoratedRepo = _proxyGenerator.CreateInterfaceProxyWithTarget(target, _cacheInterceptor);
            return decoratedRepo;
        }

        
    }


}
