using System.Collections.Generic;
using Vlerx.Es.Bdd.Tools;
using Vlerx.Es.Messaging;
using Vlerx.SampleContracts.Customers;
using Vlerx.SampleService.Customers.Commands;

namespace Vlerx.SampleService.Tests.StoryTests
{
    public class Spec
    {
        public IDomainEvent[] Given { get; set; }
        public ICommand When { get; set; }
        public IDomainEvent[] Then { get; set; }
        // public ICollection<IDomainEvent> Then { get; set; }
        public Spec(IDomainEvent[] given,
                    ICommand when,
                    IDomainEvent[] then)
        {
            this.Given = given;
            this.When = when;
            this.Then = then;
        }
    }
}