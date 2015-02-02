namespace Temporal.Core.Conventions
{
    public sealed class ConventionsFluentInterface
    {
        private readonly RepositoryDecorator _repositoryDecorator;

        public ConventionsFluentInterface(RepositoryDecorator repositoryDecorator)
        {
            _repositoryDecorator = repositoryDecorator;
        }

        public ConventionsFluentInterface Register(ICacheConvention convention)
        {
            _repositoryDecorator.CacheInterceptor.AddConvention(convention);
            return this;
        }

        public ConventionsFluentInterface Clear()
        {
            _repositoryDecorator.CacheInterceptor.ClearConventions();
            return this;
        }
    }
}
