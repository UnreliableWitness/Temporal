using System;
using Caliburn.Micro;
using Temporal.Core;
using Temporal.Wpf.Repositories;

namespace Temporal.Wpf.ViewModels
{
    public class ShellViewModel : Screen
    {
        private readonly IPersonRepository _personRepository;


        public ShellViewModel(IPersonRepository repository, IRepositoryDecorator repositoryDecorator)
        {
            _personRepository = repository;

            repositoryDecorator.InvalidateOn.CacheItemPolicySliding(TimeSpan.FromSeconds(15));
            _personRepository = repositoryDecorator.Decorate(_personRepository);
        }

        protected override void OnInitialize()
        {
            var people = _personRepository.RetrievePersons();
            people = _personRepository.RetrievePersons();
            base.OnInitialize();

        }
    }
}
