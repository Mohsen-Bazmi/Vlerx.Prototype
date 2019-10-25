using Vlerx.Es.Messaging;

namespace Vlerx.SampleContracts.Customers
{
    public class CustomerRegistered : IDomainEvent
    {
        public CustomerRegistered(string customerId, string firstName
            , string lastName, string address, string phoneNumber)
        {
            CustomerId = customerId;
            Address = address;
            PhoneNumber = phoneNumber;
            FirstName = firstName;
            LastName = lastName;
        }

        public string CustomerId { get; }
        public string Address { get; }
        public string PhoneNumber { get; }
        public string FirstName { get; }
        public string LastName { get; }
    }

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