using System;
using System.Threading.Tasks;
using Vlerx.InternalMessaging;
using Vlerx.SampleContracts.Customers;
using Vlerx.SampleReadSide.ProjectionPersistance;

namespace Vlerx.SampleReadSide.Customers
{
    public class CustomerEventListener
        : IListenTo<CustomerRegistered>
            , IListenTo<CustomerRelocated>
            , IListenTo<CustomerContactInfoChanged>
    {
        readonly IAtomicWriter<string, CustomerViewModel> _writer;
        public CustomerEventListener(IAtomicWriter<string, CustomerViewModel> writer)
        {
            _writer = writer;
        }
        public Task On(CustomerContactInfoChanged contactInfo)
        {
            Console.WriteLine($"Customer: {contactInfo.CustomerId} ContactInfo changed.");
            return _writer.UpdateAsync(contactInfo.CustomerId
                                     , c => c.PhoneNumber = contactInfo.NewPhoneNumber);
        }

        public Task On(CustomerRegistered registration)
        {
            Console.WriteLine($"Customer: {registration.FirstName} {registration.LastName} Registered.");
            return _writer.AddAsync(registration.CustomerId,
                            new CustomerViewModel
                            {
                                Id = registration.CustomerId,
                                Address = registration.Address,
                                PhoneNumber = registration.PhoneNumber,
                                FirstName = registration.FirstName,
                                LastName = registration.LastName,
                            });
        }

        public Task On(CustomerRelocated relocation)
        {
            Console.WriteLine($"Customer: {relocation.CustomerId} Relocated.");
            return _writer.UpdateAsync(relocation.CustomerId
                                     , c => c.Address = relocation.NewAddress);
        }
    }
}