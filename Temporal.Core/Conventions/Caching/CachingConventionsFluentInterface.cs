namespace Temporal.Core.Conventions.Caching
{
    public sealed class CachingConventionsFluentInterface
    {
        private readonly RepositoryDecorator _repositoryDecorator;

        public CachingConventionsFluentInterface(RepositoryDecorator repositoryDecorator)
        {
            _repositoryDecorator = repositoryDecorator;
            AddCondition(new DefaultCachingConvention());
        }

        public CachingConventionsFluentInterface AddCondition(ICacheConvention convention)
        {
            _repositoryDecorator.CacheInterceptor.AddConvention(convention);
            return this;
        }

        public CachingConventionsFluentInterface Clear()
        {
            _repositoryDecorator.CacheInterceptor.ClearConventions();
            return this;
        }
    }
}
