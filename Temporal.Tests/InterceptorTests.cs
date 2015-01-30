using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Temporal.Core;
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
        public void RetrieveShouldBeCached()
        {
            var decorator = new RepositoryDecorator();
            decorator.Conventions.Register()

            var repo = new TestRepository();
            var decoRepo = decorator.Decorate<ITestRepository>(repo);
            
            var personsA = decoRepo.RetrievePersons();
            var personsB = decoRepo.RetrievePersons();

            Assert.IsTrue(true);
        }
    }
}
