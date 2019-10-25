using System;
using System.Collections.Generic;
using System.Linq;
using Vlerx.Es.Messaging;

namespace Vlerx.Es.Persistence
{
    public sealed class UnitOfWork<TState>
        where TState : class
    {
        private UnitOfWork(string contextMessageId
            , string contextCorrelationId
            , string contextCausationId
            , IEnumerable<IDomainEvent> changes
            , TState state)
        {
            ContextMessageId = contextMessageId;
            ContextCorrelationId = contextCorrelationId;
            ContextCausationId = contextCausationId;
            Changes = changes.Select(change =>
                new UntypedEventEnvelope(Guid.NewGuid()
                    , new UntypedEventEnvelope.EventMetadata(contextCorrelationId
                        , contextCausationId)
                    , change)).ToArray();
            State = state;
        }

        public TState State { get; }
        public UntypedEventEnvelope[] Changes { get; }
        public string ContextMessageId { get; }

        private string ContextCorrelationId { get; }
        private string ContextCausationId { get; }

        public static UnitOfWork<TState> For(string currentMessageId
            , string currentCorrelationId
            , string currentCausationId
            , TState initialState)
        {
            return new UnitOfWork<TState>(currentMessageId
                , currentCorrelationId
                , currentCausationId
                , new IDomainEvent[0]
                , initialState);
        }

        public UnitOfWork<TState> Track(IEnumerable<IDomainEvent> changes
            , TState state)
        {
            return new UnitOfWork<TState>(ContextMessageId
                , ContextCorrelationId
                , ContextCausationId
                , changes
                , state);
        }
    }
}