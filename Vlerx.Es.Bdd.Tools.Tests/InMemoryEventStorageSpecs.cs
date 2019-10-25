using System;
using System.Linq;
using Vlerx.Es.Bdd.Tools.Tests.Fakes;
using Vlerx.Es.Persistence;
using Xunit;

namespace Vlerx.Es.Bdd.Tools.Tests
{
    public class InMemoryEventStorageSpecs
    {
        private static UntypedEventEnvelope CreateUntypedEventEnvelope()
        {
            var domainEvent = new FakeDomainEvent();
            var correlationId = Guid.NewGuid().ToString();
            var causationId = Guid.NewGuid().ToString();
            var eventMetadata = new UntypedEventEnvelope.EventMetadata(correlationId, causationId);
            var envelopedEvent = new UntypedEventEnvelope(domainEvent.EventId, eventMetadata, domainEvent);
            return envelopedEvent;
        }

        private static UntypedEventEnvelope[] CreateManyUntypedEventEnvelopes(int count)
        {
            return Enumerable.Range(1, count).Select(_ => CreateUntypedEventEnvelope()).ToArray();
        }

        [Theory]
        [InlineData("streamName1", 1)]
        [InlineData("streamName2", 2)]
        public void ReadsTheAppendedEvents(string streamName, int version)
        {
            var sut = new InMemoryEventStorage();

            var expectedEvents = CreateManyUntypedEventEnvelopes(10);


            sut.AppendEvents(streamName, version, expectedEvents);

            var actualEvents = sut.ReadEvents(streamName).Result;

            Assert.Equal(expectedEvents, actualEvents);
        }

        [Theory]
        [InlineData("A stream name to append to", "A non existing stream name to read from")]
        public void DoesNotThrowStreamNotFoundException(string writeStreamName, string readStreamName)
        {
            var sut = new InMemoryEventStorage();
            var expectedEvents = CreateManyUntypedEventEnvelopes(10);

            sut.AppendEvents(writeStreamName
                , 1
                , expectedEvents);

            var actualEvents = sut.ReadEvents(readStreamName).Result;
            Assert.Equal(expectedEvents, actualEvents);
//            Assert.ThrowsAsync<InMemoryEventStorage.StreamNotFoundException>(() =>
//                sut.ReadEvents(readStreamName));
        }
    }
}