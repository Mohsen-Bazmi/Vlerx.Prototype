using Vlerx.Es.StoryBroker;
using Xunit;

namespace Vlerx.Es.Bdd.Tools.Tests
{
    public class ScenariosTests
    {
        [Fact]
        public void OfStories_ScenariosOfStoriesCreatedWithInMemoryStorage()
        {
            var stories = Stories.OfUseCases();
            var scenarios = Scenarios.OfStories(eventStorage =>
            {
                Assert.IsType<InMemoryEventStorage>(eventStorage);
                return stories;
            });
        }
    }
}