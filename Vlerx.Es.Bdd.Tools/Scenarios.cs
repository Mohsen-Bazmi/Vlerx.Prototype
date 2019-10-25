using System;
using Vlerx.Es.DataStorage;
using Vlerx.Es.StoryBroker;

namespace Vlerx.Es.Bdd.Tools
{
    public static class Scenarios
    {
        public static IBdd OfStories(Func<IEventStorage, IStories> setupStories)
        {
            var storage = new InMemoryEventStorage();
            return new ScenariosOfStories(storage, setupStories(storage));
        }
    }
}