using System;
using Vlerx.Es.Messaging;

namespace Vlerx.Es.Persistence
{
    public class UntypedEventEnvelope
    {
        public UntypedEventEnvelope(Guid eventId
            , EventMetadata metadata
            , IDomainEvent body)
        {
            Id = eventId;
            Metadata = metadata;
            Body = body;
        }

        public EventMetadata Metadata { get; }
        public Guid Id { get; }
        public IDomainEvent Body { get; }

        public class EventMetadata
        {
            public EventMetadata(string correlationId
                , string causationId)
            {
                CorrelationId = correlationId;
                CausationId = causationId;
            }

            public string CorrelationId { get; }
            public string CausationId { get; }
        }
    }
}