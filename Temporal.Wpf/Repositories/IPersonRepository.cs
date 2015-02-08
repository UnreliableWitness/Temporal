using System.Collections.Generic;
using Temporal.Wpf.Models;

namespace Temporal.Wpf.Repositories
{
    public interface IPersonRepository
    {
        IEnumerable<Person> RetrievePersons();
        int Create(Person person);
        void Update(Person person);
        void Delete(int id);
    }
}