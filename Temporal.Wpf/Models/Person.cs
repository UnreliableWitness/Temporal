using System.Collections.Generic;
using Caliburn.Micro;

namespace Temporal.Wpf.Models
{
    public class Person : PropertyChangedBase
    {
        private string _first;
        private string _last;
        private int _id;

        public int Id
        {
            get { return _id; }
            set
            {
                if (value == _id) return;
                _id = value;
                NotifyOfPropertyChange(() => Id);
            }
        }

        public string First
        {
            get { return _first; }
            set
            {
                if (value == _first) return;
                _first = value;
                NotifyOfPropertyChange(() => First);
            }
        }

        public string Last
        {
            get { return _last; }
            set
            {
                if (value == _last) return;
                _last = value;
                NotifyOfPropertyChange(() => Last);
            }
        }

        public List<Address> Addresses { get; set; }

        public Person()
        {
            Addresses = new List<Address>();
        }
    }
}
