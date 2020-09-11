using Vlerx.Es.Messaging;
using Vlerx.SampleContracts.Customers;
using Vlerx.SampleService.Customers.Commands;

namespace Vlerx.SampleService.Tests.StoryTests
{
    public abstract class AbstractSpec2
    {
        public IDomainEvent[] Given { get; }
        public ICommand When { get; }
        public IDomainEvent[] Then { get; }
        protected abstract (IDomainEvent[] Given, ICommand When, IDomainEvent[] Then) Test { get; }
    }
    public class newclass : AbstractSpec2
    {
        protected override (IDomainEvent[] Given, ICommand When, IDomainEvent[] Then) Test =>
        (
            Given: new IDomainEvent[]{
                    new CustomerRegistered("customerId"
                                            , "Mohsen"
                                            , "Bazmi"
                                            , "My Old Address"
                                            , "09100000000")
            },
            When: new RelocateCustomer("customerId", "newAddress", "newPhoneNumber"),
            Then: new IDomainEvent[]{
                new CustomerRelocated("customerId", "newAddress"),
                new CustomerContactInfoChanged("customerId", "newPhoneNumber")
            }
        );
    }
}