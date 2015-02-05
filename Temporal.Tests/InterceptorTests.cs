using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Temporal.Core;
using Temporal.Core.Conventions.Caching;
using Temporal.Core.Conventions.Invalidation;
using Temporal.Core.Exceptions;
using Temporal.Tests.Fakes;
using Temporal.Tests.Mocks;

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

        [TestMethod]
        public void AbsoluteExpirationExpiredShouldNotBeEqual()
        {
            var decorator = new RepositoryDecorator();
            decorator.InvalidateOn.CacheItemPolicyAbsolute(new DateTimeOffset(DateTime.Now.AddSeconds(1)));

            var decoRepo = decorator.Decorate<ITestRepository>(new TestRepository());

            var personsA = decoRepo.RetrievePersons();
            Thread.Sleep(1000);
            var personsB = decoRepo.RetrievePersons();

            Assert.AreNotEqual(personsA, personsB);
        }

        [TestMethod]
        public void AbsoluteExpirationNotExpiredShouldBeEqual()
        {
            var decorator = new RepositoryDecorator();
            decorator.InvalidateOn.CacheItemPolicyAbsolute(new DateTimeOffset(DateTime.Now.AddSeconds(1)));

            var decoRepo = decorator.Decorate<ITestRepository>(new TestRepository());

            var personsA = decoRepo.RetrievePersons();
            var personsB = decoRepo.RetrievePersons();

            Assert.AreEqual(personsA, personsB);
        }

        [TestMethod]
        public void MethodInvocationShouldInvalidateEvenWhenCacheItemPolicyIsntTriggered()
        {
            var decorator = new RepositoryDecorator();
            decorator.InvalidateOn.CacheItemPolicySliding(TimeSpan.FromSeconds(5)).MethodInvocation(new DefaultMethodInvalidationConvention());

            var decoRepo = decorator.Decorate<ITestRepository>(new TestRepository());

            var personsA = decoRepo.RetrievePersons();
            decoRepo.UpdatePerson(personsA.First());
            var personsB = decoRepo.RetrievePersons();

            Assert.AreNotEqual(personsA, personsB);
        }

        [TestMethod]
        public void RepositoriesDecoratedBySameDecoratorShouldHaveSharedCache()
        {
            var decorator = new RepositoryDecorator();
            decorator.InvalidateOn.CacheItemPolicySliding(TimeSpan.FromMinutes(5));

            var repoA = decorator.Decorate<ITestRepository>(new TestRepository());
            var repoB = decorator.Decorate<ITestRepository>(new TestRepository());

            var repoAResults = repoA.RetrievePersons();
            var repoBResults = repoB.RetrievePersons();

            Assert.AreSame(repoAResults.First(), repoBResults.First());
        }

        [TestMethod]
        public void RepositoriesDecoratedByDifferentDecoratorShouldHaveSharedCache()
        {
            var decoratorA = new RepositoryDecorator();
            decoratorA.InvalidateOn.CacheItemPolicySliding(TimeSpan.FromMinutes(5));

            var repoA = decoratorA.Decorate<ITestRepository>(new TestRepository());

            var decoratorB = new RepositoryDecorator();
            decoratorB.InvalidateOn.CacheItemPolicySliding(TimeSpan.FromMinutes(5));
            var repoB = decoratorB.Decorate<ITestRepository>(new TestRepository());

            var repoAResults = repoA.RetrievePersons();
            var repoBResults = repoB.RetrievePersons();

            Assert.AreSame(repoAResults.First(), repoBResults.First());
        }

        [TestMethod]
        public void MultiThreadedCacheReadShouldYieldConsistentResults()
        {
            var decorator = new RepositoryDecorator();
            var repository = decorator.Decorate<ITestRepository>(new TestRepository());
            
            var taskList = new List<Task>();
            var resultList = new List<Person>();
            for (int i = 0; i < 1000; i++)
            {
                taskList.Add(Task.Run(async () =>
                {
                    //Debug.WriteLine(Task.CurrentId);
                    var people = (await repository.RetrievePersonsAsync());
                    resultList.Add(people.Single());
                    await Task.Delay(TimeSpan.FromSeconds(1));
                }));
            }

            Task.WaitAll(taskList.ToArray());

            Debug.WriteLine("{0} items in the result list",taskList.Count);

            Assert.AreEqual(resultList.Count, 1000);
        }

        [TestMethod]
        public void MultiThreadedCacheWriteShouldNotFail()
        {
            var decorator = new RepositoryDecorator();
            var repository = decorator.Decorate<ITestRepository>(new TestRepository());

            var taskList = new List<Task>();
            var resultList = new List<Person>();

            for (int i = 0; i < 1000; i++)
            {
                taskList.Add(Task.Run(() =>
                {
                     resultList.Add(repository.RetrievePerson(i));
                }));
            }

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
