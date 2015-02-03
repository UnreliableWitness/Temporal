using System;
using System.Linq;
using System.Reflection;
using System.Threading;
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

        [TestMethod]
        public void ChangeCacheItemPolicyShouldNotThrowException()
        {
            var decorator = new RepositoryDecorator();
            decorator.InvalidateOn.CacheItemPolicySliding(TimeSpan.FromSeconds(1));

            var decoRepo = decorator.Decorate<ITestRepository>(new TestRepository());

            var personsA = decoRepo.RetrievePersons();
            Thread.Sleep(1000);
            var personsB = decoRepo.RetrievePersons();

            Assert.AreNotEqual(personsA, personsB);
        }

        [TestMethod]
        public void SlidingCachePolicyIsActuallySliding()
        {
            var decorator = new RepositoryDecorator();
            decorator.InvalidateOn.CacheItemPolicySliding(TimeSpan.FromSeconds(1));

            var decoRepo = decorator.Decorate<ITestRepository>(new TestRepository());

            var personsA = decoRepo.RetrievePersons();
            Thread.Sleep(500);
            var personsB = decoRepo.RetrievePersons();

            Assert.AreEqual(personsA, personsB);

            Thread.Sleep(1000);
            personsB = decoRepo.RetrievePersons();

            Assert.AreNotEqual(personsA, personsB);
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
