using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Temporal.Wpf.Models;

namespace Temporal.Wpf.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private IEnumerable<Person> _persons; 

        public PersonRepository()
        {
            _persons = new List<Person>();
            Seed();
        }

        private void Seed()
        {
            var list = new List<Person>();
            list.Add(new Person
            {
                Id = 1,
                First = "John",
                Last = "Doe",
                Addresses = new List<Address>(new []{new Address{City = "Oudenaarde", Street = "Markt"}})
            });
            list.Add(new Person
            {
                Id = 2,
                First = "Dries",
                Last = "Hoebeke"
            });

            _persons = list;
        }

        public IEnumerable<Person> RetrievePersons()
        {
            Thread.Sleep(2000);
            return _persons;
        }

        public int Create(Person person)
        {
            person.Id = _persons.ToList().Max(p => p.Id) + 1;
            _persons.ToList().Add(person);

            return person.Id;
        }

        public void Update(Person person)
        {
            var existing = _persons.FirstOrDefault(p => p.Id == person.Id);
            if (existing == null)
                throw new KeyNotFoundException("id");

            existing = person;
        }

        public void Delete(int id)
        {
            var existing = _persons.FirstOrDefault(p => p.Id == id);
            if (existing == null)
                throw new KeyNotFoundException("id");

            _persons.ToList().Remove(existing);
        }

    }
}
