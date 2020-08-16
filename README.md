# Vlerx.Prototype
An extensible reactive event sourcing boilerplate.

How to test the behaviour using [BDD tools](https://github.com/Vlerx/Vlerx.Prototype/tree/master/Vlerx.Es.Bdd.Tools):
```cs
[Theory]
[InlineData("My Id", "09100000000", "09101111111")]
public void RelocateCustomer_CustomerRelocatedAndContactInfoChanged(string customerId, string newAddress, string newPhoneNumber)
{
    Given(new CustomerRegistered(customerId
                                , firstName: "Mohsen"
                                , lastName: "Bazmi"
                                , address: "My Old Address"
                                , phoneNumber: "09100000000"));
    When(new RelocateCustomer(customerId, newAddress, newPhoneNumber));
    Then(new CustomerRelocated(customerId, newAddress)
        , new CustomerContactInfoChanged(customerId, newPhoneNumber));
}
```
How to define use cases (application layer):
```cs
public class CustomerUseCases : UseCasesOf<Customer.State>
{
        public CustomerUseCases(IRepository<Customer.State> repository) : base(repository)
        {
            StoryOf((RegisterCustomer cmd) => Customer.Register(cmd.CustomerId
                                                               , cmd.FirstName
                                                               , cmd.LastName
                                                               , cmd.Address
                                                               , cmd.PhoneNumber));

            StoryOf<RelocateCustomer>((customer, cmd) => customer.Relocate(cmd.NewAddress
                                                                          , cmd.NewPhoneNumber));

        }
        ...
}
```
Aggregate root with it's state separated from it's logic:
```cs
    public static class Customer
    {
        public static State.Transition Register(string customerId
            , string firstName
            , string lastName
            , string address
            , string phoneNumber)
        => new State(Guid.NewGuid().ToString())
            .Apply(new CustomerRegistered(customerId
                , firstName
                , lastName
                , address
                , phoneNumber));



        public static State.Transition Relocate(this State customer
            , string newAddress
            , string newPhoneNumber)
        {
            var events = new List<IDomainEvent>();
            if (!string.IsNullOrWhiteSpace(newAddress) && customer.Address != newAddress)
                events.Add(new CustomerRelocated(customer.Id, newAddress));

            if (!string.IsNullOrWhiteSpace(newPhoneNumber))
                events.Add(new CustomerContactInfoChanged(customer.Id, newPhoneNumber));
            return customer.Apply(events);
        }

        public class State : AggregateState<State>
        {
            public State(string id)
            => Id = id;

            public string Name { get; private set; }
            public string Address { get; private set; }
            public string PhoneNumber { get; private set; }

            public void On(CustomerRegistered e)
            {
                Id = e.CustomerId;
                Name = e.FirstName + " " + e.LastName;
                Address = e.Address;
                PhoneNumber = e.PhoneNumber;
            }

            public void On(CustomerRelocated e)
            => Address = e.NewAddress;

            public void On(CustomerContactInfoChanged e)
            => PhoneNumber = e.NewPhoneNumber;


            public override string StreamNameForId(string id)
            => $"Customer-{id}";
        }
    }
```
Projection:
```cs
    public class CustomerEventListener : IListenTo<CustomerRegistered>
                                       , IListenTo<CustomerRelocated>
                                       , IListenTo<CustomerContactInfoChanged>
    {
        readonly IAtomicWriter<string, CustomerViewModel> _writer;
        public CustomerEventListener(IAtomicWriter<string, CustomerViewModel> writer)
        {
            _writer = writer;
        }
        public Task On(CustomerContactInfoChanged contactInfo)
        => _writer.UpdateAsync(contactInfo.CustomerId
                              , c => c.PhoneNumber = contactInfo.NewPhoneNumber);
        ...
    }
```
Sample Application is [available](https://github.com/Vlerx/Vlerx.Prototype/tree/master/Sample):

Before running the sample call `docker-compose up` from [env](https://github.com/Vlerx/Vlerx.Prototype/tree/master/Sample/env) folder.
