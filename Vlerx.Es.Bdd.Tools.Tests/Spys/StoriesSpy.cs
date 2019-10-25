using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vlerx.Es.Messaging;
using Vlerx.InternalMessaging;

namespace Vlerx.Es.Bdd.Tools.Tests.Spys
{
    public class StoriesSpy : IStories
    {
        private readonly List<CommandEnvelope> _recordedCommands = new List<CommandEnvelope>();
        public IEnumerable<ICommand> RecordedCommandPayloads => _recordedCommands.Select(cmd => cmd.Payload);

        public Task ContinueWith(CommandEnvelope command)
        {
            throw new NotImplementedException();
        }

        public Task Tell<TCommand>(TCommand command) where TCommand : ICommand
        {
            _recordedCommands.Add(new CommandEnvelope(MessageChainInfo.Init(), command));
            return Task.CompletedTask;
        }
    }
}