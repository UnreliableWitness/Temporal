using Microsoft.VisualStudio.TestTools.UnitTesting;
using Temporal.Core;
using Temporal.Tests.Fakes;
using Temporal.Tests.Mocks;

namespace Temporal.Tests
{
    [TestClass]
    public class CacheKeyGeneratorTests
    {
        [TestMethod]
        public void RegularRetrieveCacheKeyGetsGenerated()
        {
            var repo = new TestRepository();
            var generator = new CacheKeyGenerator();
            string key;
            generator.TryBuildCacheKey(() => repo.RetrievePersons(), out key);

            Assert.AreEqual("RetrievePersons", key);
        }

        [TestMethod]
        public void AsyncRetrieveCacheKeyGetsGenerated()
        {
            var repo = new TestRepository();
            var generator = new CacheKeyGenerator();
            string key;
            generator.TryBuildCacheKey(() => repo.RetrievePersonsAsync(), out key);

            Assert.AreEqual("RetrievePersonsAsync", key);
        }

        [TestMethod]
        public void RegularRetrieveWithArgumentsGetsGenerated()
        {
            var repo = new TestRepository();
            var generator = new CacheKeyGenerator();
            string key;
            generator.TryBuildCacheKey(() => repo.RetrievePerson(1), out key);

            Assert.AreEqual("RetrievePerson#1", key);
        }

        [TestMethod]
        public void RegularRetrieveWithNonPrimitiveArgumentShouldNotGenerateKey()
        {
            var repo = new TestRepository();
            var generator = new CacheKeyGenerator();
            string key;
            var result = generator.TryBuildCacheKey(() => repo.RetrievePerson(new Person()), out key);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void RetrieveMultipleArgumentsWithNonPrimitiveArgumentShouldNotGenerateKey()
        {
            var repo = new TestRepository();
            var generator = new CacheKeyGenerator();
            string key;
            var result = generator.TryBuildCacheKey(() => repo.RetrievePerson(new Person(), 1), out key);

            Assert.IsFalse(result);
        }
    }



}
