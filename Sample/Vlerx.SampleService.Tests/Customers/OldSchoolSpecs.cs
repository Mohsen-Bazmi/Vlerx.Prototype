using System;
using System.Linq;
using Vlerx.Es.Bdd.Tools;
using Vlerx.Es.DataStorage;
using Vlerx.Es.Messaging;
using Vlerx.Es.Persistence;
using Vlerx.Es.StoryBroker;
using Vlerx.SampleContracts.Customers;
using Vlerx.SampleService.Customers;
using Vlerx.SampleService.Customers.Commands;
using Xunit;

namespace Vlerx.SampleService.Tests.Customers
{
    public class OldSchoolSpecs
    {
        [Fact(Skip = "For Sample")]
        public void RegisterCustomer_CustomerRegistered()
        {
            IEventStorage eventStorage = new InMemoryEventStorage();
            var repository = new Repository<Customer.State>(eventStorage);
            var useCases = new CustomerUseCases(repository);

            var stories = Stories.OfUseCases(useCases);

            var customerId = Guid.NewGuid().ToString();
            var firstName = "Mohsen";
            var lastName = "Bazmi";
            var address = "Customer Address";
            var phoneNumber = "09100000000";
            var command = new RegisterCustomer(customerId
                , firstName
                , lastName
                , address
                , phoneNumber);
            var expectedEvents = new IDomainEvent[]
            {
                new CustomerRegistered(customerId, firstName, lastName, address, phoneNumber)
            };

//            var envelopedCommand = new CommandEnvelope(MessageChainInfo.Init(), command);
            stories.Tell(command);

            var events = eventStorage
                .ReadEvents(new Customer.State(customerId).StreamNameForId(customerId))
                .Result
                .Select(e => e.Body);
            Assert.Equal(events, expectedEvents);
        }
    }
}