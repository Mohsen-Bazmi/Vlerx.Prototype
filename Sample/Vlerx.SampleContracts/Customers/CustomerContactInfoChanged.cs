using Vlerx.Es.Messaging;

namespace Vlerx.SampleContracts.Customers
{
    public class CustomerContactInfoChanged : IDomainEvent
    {
        public CustomerContactInfoChanged(string customerId, string newPhoneNumber)
        {
            CustomerId = customerId;
            NewPhoneNumber = newPhoneNumber;
        }

        public string CustomerId { get; }
        public string NewPhoneNumber { get; }
    }
}