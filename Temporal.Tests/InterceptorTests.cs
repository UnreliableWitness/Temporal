using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Temporal.Core;
using Temporal.Core.Conventions.Caching;
using Temporal.Core.Conventions.Invalidation;
using Temporal.Core.Exceptions;
using Temporal.Tests.Fakes;

namespace Temporal.Tests
{
    [TestClass]
    public class InterceptorTests
    {
        [TestMethod]
        public void NoCacheAttributeShouldIgnoreCacher()
        {
            var decorator = new RepositoryDecorator();

            var repo = new TestRepository();
            var decoRepo = decorator.Decorate<ITestRepository>(repo);

            var persons = decoRepo.RetrievePersonsNoCache();

            Assert.IsNotNull(persons);
            Assert.AreSame(persons.First().First, "Dries");
        }

        [TestMethod]
        public void DefaultConventionsShouldAllowCache()
        {
            var decorator = new RepositoryDecorator();

            var repo = new TestRepository();
            var decoRepo = decorator.Decorate<ITestRepository>(repo);
            
            var personsA = decoRepo.RetrievePersons();
            var personsB = decoRepo.RetrievePersons();

            Assert.AreEqual(personsA, personsB);
        }

        [TestMethod]
        public void ExtraConventionsShouldAllowCache()
        {
            var decorator = new RepositoryDecorator();
            decorator.CacheIf.AddCondition(new TestConvention());

            var repo = new TestRepository();
            var decoRepo = decorator.Decorate<ITestRepository>(repo);

            var personsA = decoRepo.SelectPersons();
            var personsB = decoRepo.SelectPersons();

            Assert.AreEqual(personsA, personsB);
        }

        [TestMethod]
        [ExpectedException(typeof(ConventionAlreadyRegisteredException))]
        public void DoubleConventionRegistrationShouldThrowException()
        {
            var decorator = new RepositoryDecorator();
            decorator.CacheIf.AddCondition(new DefaultCachingConvention());
        }

        [TestMethod]
        public void InvalidationConventionShouldBeAbleToBeConfigured()
        {
            var decorator = new RepositoryDecorator();
            decorator.InvalidateOn.MethodInvocation(new DefaultMethodInvalidationConvention());
        }

        [TestMethod]
        public void ShouldInvalidateOnDefaultMethodInvalidationConvention()
        {
            var decorator = new RepositoryDecorator();
            decorator.InvalidateOn.MethodInvocation(new DefaultMethodInvalidationConvention());

            var repo = new TestRepository();
            var decoRepo = decorator.Decorate<ITestRepository>(repo);
            

            var personsA = decoRepo.RetrievePersons();
            var personsB = decoRepo.RetrievePersons();

            Assert.AreEqual(personsA, personsB);

            var person = personsA.FirstOrDefault();
            decoRepo.UpdatePerson(person);

            

        }
    }

    public class TestConvention : ICacheConvention
    {
        public bool ShouldCache(MethodInfo methodInfo)
        {
            return methodInfo.Name.StartsWith("Select");
        }
    }
}
