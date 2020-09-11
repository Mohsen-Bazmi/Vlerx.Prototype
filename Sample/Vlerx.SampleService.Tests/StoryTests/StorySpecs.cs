using System.Collections.Generic;
using Vlerx.Es.Bdd.Tools;
using Vlerx.Es.Messaging;
using Vlerx.SampleContracts.Customers;
using Vlerx.SampleService.Customers.Commands;

namespace Vlerx.SampleService.Tests.StoryTests
{
    public partial class StorySpecs
    {
        public static Spec[] RestaurantSpecs = new Spec[]
        {
            //new Spec\("4455232123165798798797964654f"\)\n+\s+\{\n+\s+Given*\s+=\s+new\s+IDomainEvent
            
            new Spec//Id: "4455232123165798798797964654f"
            (
                given : new IDomainEvent[]{
                    new CustomerRegistered("customerId"
                                            , "Mohsen"
                                            , "Bazmi"
                                            , "My Old Address"
                                            , "09100000000")
                },
                when : new RelocateCustomer("customerId", "newAddress", "newPhoneNumber"),
                then : new IDomainEvent[]{
                    new CustomerRelocated("customerId", "newAddress"),
                    new CustomerContactInfoChanged("customerId", "newPhoneNumber")
                }
            ),
        };
    }
}