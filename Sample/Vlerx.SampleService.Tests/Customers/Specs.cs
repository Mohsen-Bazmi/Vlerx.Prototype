using Vlerx.Es.Bdd.Tools;
using Vlerx.Es.Persistence;
using Vlerx.Es.StoryBroker;
using Vlerx.SampleContracts.Customers;
using Vlerx.SampleService.Customers;
using Vlerx.SampleService.Customers.Commands;
using Xunit;

namespace Vlerx.SampleService.Tests.Customers
{
    public class CustomerSpecs : BehaviouralSpecs
    {
        public CustomerSpecs() 
        : base(Scenarios.OfStories(es => Stories.OfUseCases(
                new CustomerUseCases(new Repository<Customer.State>(es))
            )))
        {
        }

        [Theory]
        [InlineData("My Id", "Mohsen", "Bazmi", "My Address", "09100000000")]
        public void RegisterCustomer_CustomerRegistered(string customerId
            , string firstName
            , string lastName
            , string address
            , string phoneNumber)
        {
            When(new RegisterCustomer(customerId
                , firstName
                , lastName
                , address
                , phoneNumber));
            Then(new CustomerRegistered(customerId
                , firstName
                , lastName
                , address
                , phoneNumber));
        }

        [Theory]
        [InlineData("My Id", "09100000000", "09101111111")]
        public void RelocateCustomer_CustomerRelocatedAndContactInfoChanged(
            string customerId
            , string newAddress
            , string newPhoneNumber)
        {
            Given(new CustomerRegistered(customerId
                , "Mohsen"
                , "Bazmi"
                , "My Old Address"
                , "09100000000"));
            When(new RelocateCustomer(customerId, newAddress, newPhoneNumber));
            Then(new CustomerRelocated(customerId, newAddress)
                , new CustomerContactInfoChanged(customerId, newPhoneNumber));
        }

        [Theory]
        [InlineData("My Id", "", "09101111111")]
        public void RelocateCustomer_WithoutAddress_CustomerContactInfoChanged(
            string customerId
            , string oldAddress
            , string newPhoneNumber)
        {
            Given(new CustomerRegistered(customerId
                , "Mohsen"
                , "Bazmi"
                , "My Old Address"
                , "09100000000"));
            When(new RelocateCustomer(customerId, oldAddress, newPhoneNumber));
            Then(new CustomerContactInfoChanged(customerId, newPhoneNumber));
        }

        [Theory]
        [InlineData("My Id", "My Old Address", "09101111111")]
        public void RelocateCustomer_WithOldAddress_CustomerContactInfoChanged(
            string customerId
            , string oldAddress
            , string newPhoneNumber)
        {
            Given(new CustomerRegistered(customerId
                , "Mohsen"
                , "Bazmi"
                , oldAddress
                , "09100000000"));
            When(new RelocateCustomer(customerId, oldAddress, newPhoneNumber));
            Then(new CustomerContactInfoChanged(customerId, newPhoneNumber));
        }

        [Theory]
        [InlineData("My Id", "My New Address", "")]
        public void RelocateCustomer_WithoutPhoneNumber_CustomerRelocated(
            string customerId
            , string newAddress
            , string newPhoneNumber)
        {
            Given(new CustomerRegistered(customerId
                , "Mohsen"
                , "Bazmi"
                , "My Old Address"
                , "09100000000"));
            When(new RelocateCustomer(customerId, newAddress, newPhoneNumber));
            Then(new CustomerRelocated(customerId, newAddress));
        }

        [Theory]
        [InlineData("My Id", "", "")]
        public void RelocateCustomer_WithoutAddressAndPhoneNumber_NothingHappened(
            string customerId
            , string newAddress
            , string newPhoneNumber)
        {
            Given(new CustomerRegistered(customerId
                , "Mohsen"
                , "Bazmi"
                , "My Old Address"
                , "09100000000"));
            When(new RelocateCustomer(customerId, newAddress, newPhoneNumber));
            Then();
        }
    }
}