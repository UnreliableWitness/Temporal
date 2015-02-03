using System.Collections.Generic;

namespace Temporal.Core.Conventions.Invalidation
{
    public sealed class InvalidationConventionsFluentInterface
    {
        private readonly RepositoryDecorator _repositoryDecorator;

        public InvalidationConventionsFluentInterface(RepositoryDecorator repositoryDecorator)
        {
            _repositoryDecorator = repositoryDecorator;
            _repositoryDecorator.CacheProvider.InvalidationConfiguration = new InvalidationConfiguration();
        }

        public InvalidationConventionsFluentInterface MethodInvocation(IEnumerable<IMethodInvalidationConvention> invalidateConventions)
        {
            _repositoryDecorator.CacheProvider.InvalidationConfiguration.MethodInvalidation = true;
            _repositoryDecorator.CacheProvider.InvalidationConfiguration.MethodInvalidationConventions.AddRange(invalidateConventions);
            return this;
        }

        public InvalidationConventionsFluentInterface MethodInvocation(IMethodInvalidationConvention invalidateConvention)
        {
            return MethodInvocation(new[] {invalidateConvention});
        }


        public InvalidationConventionsFluentInterface TimeElapsed()
        {
            return this;
        }

        public InvalidationConventionsFluentInterface MaxCountReached(int max)
        {
            return this;
        }
    }
}
