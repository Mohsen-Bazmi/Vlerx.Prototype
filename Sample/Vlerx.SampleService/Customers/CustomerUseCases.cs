using System;
using Vlerx.Es.Messaging;
using Vlerx.Es.Persistence;
using Vlerx.Es.UseCases;
using Vlerx.SampleService.Customers.Commands;

namespace Vlerx.SampleService.Customers
{
    public class CustomerUseCases : UseCasesOf<Customer.State>
    {
        public CustomerUseCases(IRepository<Customer.State> repository) : base(repository)
        {
            StoryOf((RegisterCustomer cmd)
                => Customer.Register(
                    cmd.CustomerId
                    , cmd.FirstName
                    , cmd.LastName
                    , cmd.Address
                    , cmd.PhoneNumber
                ));

            StoryOf<RelocateCustomer>((customer, cmd) =>
                customer.Relocate(cmd.NewAddress
                    , cmd.NewPhoneNumber));
        }

        protected override Customer.State InitializeState(string id)
        {
            return new Customer.State(id);
        }

        // protected override string GetId(ICommand command)
        //     => command switch
        //     {
        //         RegisterCustomer c => c.CustomerId,
        //         RelocateCustomer c => c.CustomerId,
        //         _ => throw new Exception("Customer's id not found")
        //     };
        protected override string GetId(ICommand command)
        {
            return command switch
            {
                RegisterCustomer c => c.CustomerId,
                RelocateCustomer c => c.CustomerId,
                _ => throw new Exception("Customer's id not found")
            };
        }


        protected string GetStreamName(string id)
        {
            return $"Customer-{id}";
        }
    }
}