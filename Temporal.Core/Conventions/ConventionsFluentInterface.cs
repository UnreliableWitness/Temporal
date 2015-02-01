namespace Temporal.Core.Conventions
{
    public sealed class ConventionsFluentInterface
    {
        private readonly RepositoryDecorator _repositoryDecorator;

        public ConventionsFluentInterface(RepositoryDecorator repositoryDecorator)
        {
            _repositoryDecorator = repositoryDecorator;
        }

        public ConventionsFluentInterface Register(CacheConvention convention)
        {
            _repositoryDecorator.RegisteredConventions.Add(convention);
            return this;
        }

        public ConventionsFluentInterface Clear()
        {
            _repositoryDecorator.RegisteredConventions.Clear();
            return this;
        }
    }
}
