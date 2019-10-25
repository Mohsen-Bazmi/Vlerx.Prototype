using System;
using System.Threading.Tasks;
using Vlerx.InternalMessaging;
using Vlerx.SampleContracts.Customers;

namespace Vlerx.SampleReadSide
{
    public class CustomerEventListener
        : IListenTo<CustomerRegistered>
            , IListenTo<CustomerRelocated>
            , IListenTo<CustomerContactInfoChanged>
    {
        public Task On(CustomerContactInfoChanged message)
        {
            Console.WriteLine($"Customer: {message.CustomerId} ContactInfo changed.");
            return Task.CompletedTask;
        }

        public Task On(CustomerRegistered message)
        {
            Console.WriteLine($"Customer: {message.FirstName} {message.LastName} Registered.");
            return Task.CompletedTask;
        }

        public Task On(CustomerRelocated message)
        {
            Console.WriteLine($"Customer: {message.CustomerId} Relocated.");
            return Task.CompletedTask;
        }
    }
}