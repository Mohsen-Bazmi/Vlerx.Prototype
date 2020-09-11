using Vlerx.Es.Bdd.Tools;
using Vlerx.Es.Messaging;
using Vlerx.Es.Persistence;
using Vlerx.SampleContracts.Customers;
using Vlerx.SampleService.Customers;
using Vlerx.SampleService.Customers.Commands;
using Xunit;

namespace Vlerx.SampleService.Tests.StoryTests
{
    public class RelocatesCustomer: IStorySpecification
    {
        public IDomainEvent[] Given
        => new IDomainEvent[]{
            new CustomerRegistered(
                                  customerId: "customerId"
                                  ,firstName: "Mohsen"
                                  ,lastName: "Bazmi"
                                  ,address: "my address"
                                  ,phoneNumber: "+4455511122"),
                    
        };
        public ICommand When
        => new RelocateCustomer(
                                customerId: "customerId"
                                ,newAddress: "Oak street"
                                ,newPhoneNumber: "+44555112100");

        public IDomainEvent[] Then
        => new IDomainEvent[]{
            new CustomerRelocated(
                                 customerId: "customerId"
                                 ,newAddress: "Oak street"),
                    
        };

        public string Sut { get; } = nameof(Customer);

        [Fact]
        public void Test()
        => TestAdapter.Test(this
                , setupUseCases: eventStore =>
                     new[] {
                        new CustomerUseCases(new Repository<Customer.State>(eventStore))
                     });
    }
}