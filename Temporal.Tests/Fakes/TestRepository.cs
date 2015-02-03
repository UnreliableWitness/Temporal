using System.Collections.Generic;
using System.Threading.Tasks;
using Temporal.Core.Attributes;
using Temporal.Core.Attributes.Invalidation;
using Temporal.Tests.Mocks;

namespace Temporal.Tests.Fakes
{
    public class TestRepository : ITestRepository
    {
        public async Task<IEnumerable<Person>> RetrievePersonsAsync()
        {
            return await Task.FromResult(RetrievePersons());
        }

        public IEnumerable<Person> RetrievePersons()
        {
            var result = new List<Person>();
            result.Add(new Person { First = "Dries" });
            return result;
        }

        [DontCache]
        public IEnumerable<Person> RetrievePersonsNoCache()
        {
            var result = new List<Person>();
            result.Add(new Person { First = "Dries" });
            return result;
        }

        public Person RetrievePerson(int id)
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

        public IEnumerable<Person> SelectPersons()
        {
            var result = new List<Person>();
            result.Add(new Person { First = "Dries" });
            return result;
        }

        public byte[] UpdatePerson(Person person)
        {
            return new byte[] {};
        }

        public async Task<byte[]> UpdatePersonAsync(Person person)
        {
            return await Task.FromResult(new byte[] {});
        }
    }
}
