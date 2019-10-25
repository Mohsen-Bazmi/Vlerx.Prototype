using System;
using Vlerx.Es.Messaging;

namespace Vlerx.Es.Bdd.Tools.Tests.Fakes
{
    public class FakeCommand : ICommand
    {
        public Guid CommandId { get; } = Guid.NewGuid();
    }
}