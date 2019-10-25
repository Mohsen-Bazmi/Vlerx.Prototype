using System;
using Vlerx.Es.Messaging;

namespace Vlerx.Es.Bdd.Tools.Tests.Fakes
{
    internal class FakeDomainEvent : IDomainEvent
    {
        public Guid EventId { get; } = Guid.NewGuid();
    }
}