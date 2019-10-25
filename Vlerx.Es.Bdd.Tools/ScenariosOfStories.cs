using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;
using Vlerx.Es.DataStorage;
using Vlerx.Es.Messaging;
using Vlerx.Es.Persistence;
using Vlerx.InternalMessaging;

namespace Vlerx.Es.Bdd.Tools
{
    public class ScenariosOfStories : IBdd
    {
        private const string streamName = "";
        private readonly IEventStorage _eventStorage;

        private readonly IStories _stories;

        private readonly List<IDomainEvent> _givenEvents = new List<IDomainEvent>();

        [DebuggerStepThrough]
        public ScenariosOfStories(IEventStorage storage, IStories stories)
        {
            _eventStorage = storage;
            _stories = stories;
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
            _stories.Tell(command).Wait();
        }


        [DebuggerStepThrough]
        public void Then(params IDomainEvent[] expectedEvents)
        {
            var storedEvents = _eventStorage.ReadEvents(streamName)
                .Result
                .Select(e => e.Body)
                .ToList();

            if (AreDifferent(storedEvents, _givenEvents.ToList().Concat(expectedEvents)))
                throw new EventsDidNotHappenException(expectedEvents, storedEvents);
        }

        private static bool AreDifferent(object left, object right)
        {
            return JsonConvert.SerializeObject(left)
                   != JsonConvert.SerializeObject(right);
        }
    }
}