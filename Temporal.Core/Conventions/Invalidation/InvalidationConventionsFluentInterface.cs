using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace Temporal.Core.Conventions.Invalidation
{
    public sealed class InvalidationConventionsFluentInterface
    {
        private readonly RepositoryDecorator _repositoryDecorator;

        public InvalidationConventionsFluentInterface(RepositoryDecorator repositoryDecorator)
        {
            var defaultCacheItemPolicy = new CacheItemPolicy();
            defaultCacheItemPolicy.SlidingExpiration = TimeSpan.FromMinutes(10);

            _repositoryDecorator = repositoryDecorator;
            _repositoryDecorator.CacheProvider.InvalidationConfiguration = new InvalidationConfiguration(defaultCacheItemPolicy);
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

        public InvalidationConventionsFluentInterface CacheItemPolicySliding(TimeSpan expireAfter)
        {
            var slidingPolicy = new CacheItemPolicy
            {
                SlidingExpiration = expireAfter
            };
            _repositoryDecorator.CacheProvider.InvalidationConfiguration.CacheItemPolicy = slidingPolicy;
            return this;
        }

        public InvalidationConventionsFluentInterface CacheItemPolicyAbsolute(DateTimeOffset offset)
        {
            var absoluteExpiryPolicy = new CacheItemPolicy
            {
                AbsoluteExpiration = offset
            };
            _repositoryDecorator.CacheProvider.InvalidationConfiguration.CacheItemPolicy = absoluteExpiryPolicy;
            return this;
        }

        public InvalidationConventionsFluentInterface MaxCountReached(int max)
        {
            _repositoryDecorator.CacheProvider.InvalidationConfiguration.MaxCount = max;
            return this;
        }
    }


}
