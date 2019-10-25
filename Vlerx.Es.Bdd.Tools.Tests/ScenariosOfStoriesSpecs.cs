using System;
using System.Linq;
using Vlerx.Es.Bdd.Tools.Tests.Fakes;
using Vlerx.Es.Bdd.Tools.Tests.Spys;
using Vlerx.Es.DataStorage;
using Vlerx.Es.Messaging;
using Vlerx.Es.Persistence;
using Xunit;

namespace Vlerx.Es.Bdd.Tools.Tests
{
    public class ScenariosOfStoriesSpecs
    {
        public ScenariosOfStoriesSpecs()
        {
            _sut = new ScenariosOfStories(_storage, _storiesSpy);
        }

        private readonly IEventStorage _storage = new InMemoryEventStorage();
        private readonly StoriesSpy _storiesSpy = new StoriesSpy();
        private readonly IBdd _sut;

        [Fact]
        public void Given_AppendsEventsToStorage()
        {
            var expected = new IDomainEvent[] {new FakeDomainEvent()};

            _sut.Given(expected);

            var actual =
                _storage.ReadEvents("")
                    .Result
                    .Select(e => e.Body);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Then_DoesNotThrowWhenEventsAreTheSameAsStorage()
        {
            var events = new IDomainEvent[] {new FakeDomainEvent()};
            var untypedEvents = events.Select(e =>
                new UntypedEventEnvelope(Guid.NewGuid()
                    , new UntypedEventEnvelope.EventMetadata(Guid.NewGuid().ToString()
                        , Guid.NewGuid().ToString())
                    , e)
            ).ToArray();
            _storage.AppendEvents("", 0, untypedEvents);

            _sut.Then(events);
        }

        [Fact]
        public void Then_ThrowsWhenEventsAreDifferentFromStorage()
        {
            var storedEvents = new IDomainEvent[] {new FakeDomainEvent()};

            _storage.AppendEvents(""
                , 0
                , storedEvents.Select(e =>
                    new UntypedEventEnvelope(Guid.NewGuid()
                        , new UntypedEventEnvelope.EventMetadata(Guid.NewGuid().ToString()
                            , Guid.NewGuid().ToString())
                        , e)
                ).ToArray());

            var invalidEvents = new IDomainEvent[] {new FakeDomainEvent()};
            Assert.Throws<EventsDidNotHappenException>(
                () => _sut.Then(invalidEvents));
        }

        [Fact]
        public void When_TellsTheCommandToStories()
        {
            var command = new FakeCommand();
            _sut.When(command);

            Assert.Contains(_storiesSpy.RecordedCommandPayloads, p => p == command);
        }
    }
}