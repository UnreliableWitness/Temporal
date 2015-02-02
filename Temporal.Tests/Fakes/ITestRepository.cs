using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Temporal.Tests.Mocks;

namespace Temporal.Tests.Fakes
{
    public interface ITestRepository
    {
        Task<IEnumerable<Person>> RetrievePersonsAsync();
        IEnumerable<Person> RetrievePersons();
        IEnumerable<Person> RetrievePersonsNoCache();
        Person RetrievePerson(int id);
        Person RetrievePerson(Person person);
        Person RetrievePerson(Person person, int i);
        IEnumerable<Person> SelectPersons();
    }
}