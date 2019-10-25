using System;

namespace Vlerx.Es.Messaging
{
    public class MessageChainInfo
    {
        public MessageChainInfo(string messageId
            , string correlationId
            , string causationId = null)
        {
            MessageId = messageId;
            CorrelationId = correlationId;
            CausationId = causationId;
        }

        // public MessageChainInfo Next(string nextMessageId)
        // => new MessageChainInfo(nextMessageId, CorrelationId, MessageId);
        public string MessageId { get; }
        public string CorrelationId { get; }
        public string CausationId { get; }

        public static MessageChainInfo Init()
        {
            var messageId = Guid.NewGuid().ToString();
            return new MessageChainInfo(messageId, messageId);
        }
    }
}