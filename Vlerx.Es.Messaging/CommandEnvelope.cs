namespace Vlerx.Es.Messaging
{
    public class CommandEnvelope
    {
        public CommandEnvelope(MessageChainInfo chainInfo, ICommand payload)
        {
            ChainInfo = chainInfo;
            Payload = payload;
        }

        public MessageChainInfo ChainInfo { get; }
        public ICommand Payload { get; }
    }

//    public class CommandEnvelope<T>
//    {
//        public CommandEnvelope(MessageChainInfo chainInfo, T payload)
//        {
//            ChainInfo = chainInfo;
//            Payload = payload;
//        }
//
//        public MessageChainInfo ChainInfo { get; }
//        public T Payload { get; }
//    }
}