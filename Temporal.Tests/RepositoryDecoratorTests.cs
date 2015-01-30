using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Temporal.Core;
using Temporal.Tests.Fakes;

namespace Temporal.Tests
{
    [TestClass]
    public class RepositoryDecoratorTests
    {
        [TestMethod]
        public async Task NewFactoryShouldBeCreated()
        {
            var decorator = new RepositoryDecorator();

            var repo = new TestRepository();
            var decoRepo = decorator.Decorate<ITestRepository>(repo);

            var persons = await decoRepo.RetrievePersonsAsync();

            Assert.IsNotNull(persons);
            Assert.AreSame(persons.First().First, "Dries");
        }

        [TestMethod]
        [ExpectedException(typeof(FakeException))]
        public void RepositoryMethodShouldBeIntercepted()
        {
            var decorator = new RepositoryDecorator(new FakeCacheInterceptor());

            var repo = new TestRepository();
            var decoRepo = decorator.Decorate<ITestRepository>(repo);
            var x = decoRepo.RetrievePersons();

            Assert.IsTrue(true);
        }
    }
}
