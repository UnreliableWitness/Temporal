using System.Collections.Generic;
using System.Threading.Tasks;
using Temporal.Tests.Mocks;

namespace Temporal.Tests.Fakes
{
    public class TestRepository : ITestRepository
    {
        public async Task<IEnumerable<Person>> RetrievePersonsAsync()
        {
            return await Task.FromResult(RetrievePersons());
        }

        public virtual IEnumerable<Person> RetrievePersons()
        {
            var result = new List<Person>();
            result.Add(new Person { First = "Dries" });
            return result;
        }

        public virtual Person RetrievePerson(int id)
        {
            return new Person
            {
                First = "Dries"
            };
        }

        public Person RetrievePerson(Person person)
        {
            return new Person
            {
                First = "Dries"
            };
        }

        public Person RetrievePerson(Person person, int i)
        {
            return new Person
            {
                First = "Dries"
            };
        }
    }
}
