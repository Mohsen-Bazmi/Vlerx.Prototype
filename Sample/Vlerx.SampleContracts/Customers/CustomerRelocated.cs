using Vlerx.Es.Messaging;

namespace Vlerx.SampleContracts.Customers
{
    public class CustomerRelocated : IDomainEvent
    {
        public CustomerRelocated(string customerId, string newAddress)
        {
            CustomerId = customerId;
            NewAddress = newAddress;
        }

        public string CustomerId { get; }
        public string NewAddress { get; }
    }
}