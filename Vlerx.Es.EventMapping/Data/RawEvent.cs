using System;

namespace Vlerx.Es.EventMapping.Data
{
    public class RawEvent
    {
        public RawEvent(string eventType
            , Guid eventId
            , byte[] metadata
            , byte[] data)
        {
            EventType = eventType;
            EventId = eventId;
            Metadata = metadata;
            Body = data;
        }

        public string EventType { get; }
        public Guid EventId { get; }
        public byte[] Metadata { get; }
        public byte[] Body { get; }
    }
}