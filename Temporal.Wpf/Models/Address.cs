namespace Temporal.Wpf.Models
{
    public class Address
    {
        public int Id { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public virtual Person Person { get; set; }
    }
}
