using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Temporal.Core;
using Temporal.Core.Conventions;
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
            decorator.Conventions.Register(new DefaultConvention());

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
            decorator.Conventions.Register(new DefaultConvention());
            decorator.Conventions.Register(new TestConvention());

            var repo = new TestRepository();
            var decoRepo = decorator.Decorate<ITestRepository>(repo);

            var personsA = decoRepo.SelectPersons();
            var personsB = decoRepo.SelectPersons();

            Assert.AreEqual(personsA, personsB);
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
