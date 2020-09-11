using System.Collections.Generic;
using System.Linq;
using Vlerx.Es.Bdd.Tools;
using Vlerx.Es.Persistence;
using Vlerx.Es.StoryBroker;
using Vlerx.SampleService.Customers;
using Xunit;

namespace Vlerx.SampleService.Tests.StoryTests
{
    public class StoryTests : BehaviouralSpecs
    {
        public StoryTests() :
        base(Scenarios.OfStories(eventStore => Stories.OfUseCases(
                new CustomerUseCases(new Repository<Customer.State>(eventStore))
            )))
        {
        }
        public static IEnumerable<object[]> Specs
        = StorySpecs.RestaurantSpecs.Select(s => new object[] { s }).ToList();

        [Theory]
        [MemberData(nameof(Specs))]
        public void Test_RestaurantSpecs(Spec spec)
        {
            Given(spec.Given);
            When(spec.When);
            Then(spec.Then);
        }
    }
}