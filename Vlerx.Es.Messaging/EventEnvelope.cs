namespace Vlerx.Es.Messaging
{
    public class EventEnvelope
    {
        public EventEnvelope(MessageChainInfo chainInfo, IDomainEvent payload)
        {
            ChainInfo = chainInfo;
            Payload = payload;
        }

        public MessageChainInfo ChainInfo { get; }
        public IDomainEvent Payload { get; }
    }
}