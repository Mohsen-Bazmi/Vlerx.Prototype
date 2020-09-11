using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;
using Vlerx.Es.Bdd.Tools;
using Vlerx.Es.DataStorage;
using Vlerx.Es.Messaging;
using Vlerx.Es.Persistence;
using Vlerx.Es.StoryBroker;
using Vlerx.Es.UseCases;

namespace Vlerx.SampleService.Tests.StoryTests
{
    public class TestAdapter
    {

        private const string streamName = "";
        private readonly IEventStorage _eventStorage;

        private readonly IStories _bus;

        private readonly List<IDomainEvent> _givenEvents = new List<IDomainEvent>();

        public TestAdapter(IEventStorage storage, IStories stories)
        {
            _eventStorage = storage;
            _bus = stories;
        }

        [DebuggerStepThrough]
        public void Given(params IDomainEvent[] events)
        {
            _givenEvents.AddRange(events);
            var newEvents = events.Select(e =>
                new UntypedEventEnvelope(
                    Guid.NewGuid(),
                    new UntypedEventEnvelope.EventMetadata(
                        Guid.NewGuid().ToString()
                      , Guid.NewGuid().ToString()),
                    e
                )).ToArray();
            _eventStorage.AppendEvents(streamName
                , 0
                , newEvents);
        }


        [DebuggerStepThrough]
        public void When(ICommand command)
        {
            _bus.Tell(command).Wait();
        }


        // [DebuggerStepThrough]
        public void Then(params IDomainEvent[] expectedEvents)
        {
            var storedEvents = _eventStorage.ReadEvents(streamName)
                                            .Result
                                            .Select(e => e.Body)
                                            .ToList();

            if (!Contains(storedEvents, expectedEvents))
                throw new EventsDidNotHappenException(expectedEvents, storedEvents);
        }

        static bool Contains(List<IDomainEvent> all, IDomainEvent[] subset)
        => subset.Any(expected => all.Any(actual => AreDeeplyEqual(actual, expected)));

        private static bool AreDeeplyEqual(object left, object right)
        => JsonConvert.SerializeObject(left) == JsonConvert.SerializeObject(right);

        public static void Test(IStorySpecification spec, Func<IEventStorage, IUseCase[]> setupUseCases)
        {
            var eventStore = new InMemoryEventStorage();
            var adapter = new TestAdapter(eventStore, Stories.OfUseCases(setupUseCases(eventStore)));
            adapter.Given(spec.Given);
            adapter.When(spec.When);
            adapter.Then(spec.Then);
        }
    }
}