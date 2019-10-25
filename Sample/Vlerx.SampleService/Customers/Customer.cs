using System;
using System.Collections.Generic;
using Vlerx.Es.DomainModel;
using Vlerx.Es.Messaging;
using Vlerx.SampleContracts.Customers;

namespace Vlerx.SampleService.Customers
{
    public static class Customer
    {
        public static AggregateState<State>.Transition Register(string customerId
            , string firstName
            , string lastName
            , string address
            , string phoneNumber)
        {
            return new State(Guid.NewGuid().ToString())
                .Apply(new CustomerRegistered(customerId
                    , firstName
                    , lastName
                    , address
                    , phoneNumber));
        }


        public static AggregateState<State>.Transition Relocate(this State customer
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
            {
                Id = id;
            }

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
            {
                Address = e.NewAddress;
            }

            public void On(CustomerContactInfoChanged e)
            {
                PhoneNumber = e.NewPhoneNumber;
            }


            public override string StreamNameForId(string id)
            {
                return $"Customer-{id}";
            }
        }
    }
}