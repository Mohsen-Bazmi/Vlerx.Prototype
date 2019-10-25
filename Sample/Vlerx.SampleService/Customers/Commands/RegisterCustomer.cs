using Vlerx.Es.Messaging;

namespace Vlerx.SampleService.Customers.Commands
{
    public class RegisterCustomer : ICommand
    {
        public RegisterCustomer(
            string customerId
            , string firstName
            , string lastName
            , string address
            , string phoneNumber)
        {
            CustomerId = customerId;
            FirstName = firstName;
            LastName = lastName;
            Address = address;
            PhoneNumber = phoneNumber;
        }

        public string CustomerId { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Address { get; }
        public string PhoneNumber { get; }
    }

    public class RelocateCustomer : ICommand
    {
        public RelocateCustomer(
            string customerId
            , string newAddress
            , string newPhoneNumber)
        {
            CustomerId = customerId;
            NewAddress = newAddress;
            NewPhoneNumber = newPhoneNumber;
        }

        public string CustomerId { get; }
        public string NewAddress { get; }
        public string NewPhoneNumber { get; }
    }
}