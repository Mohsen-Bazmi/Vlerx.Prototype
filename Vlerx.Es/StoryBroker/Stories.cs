using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vlerx.Es.Messaging;
using Vlerx.Es.UseCases;

namespace Vlerx.Es.StoryBroker
{
    public sealed class Stories : IStories
    {
        private readonly IUseCase[] _useCases;

        private Stories(params IUseCase[] useCases)
        {
            //TODO: guard for redundancy
            _useCases = useCases;
        }


        public Task ContinueWith(CommandEnvelope command)
        {
            var commandType = command.GetType();
            var useCase = _useCases.FirstOrDefault(uc => uc.CanTell(command.Payload));
            if (null == useCase)
                throw new NoStoryException(command.Payload.GetType().Name);
            return useCase.Tell(command);
        }

        public Task Tell<TCommand>(TCommand command)
            where TCommand : ICommand
        {
            return ContinueWith(Envelope(command));
        }

        public static IStories OfUseCases(params IUseCase[] useCases)
        {
            return new Stories(useCases);
        }

        public static IStories OfUseCases(IEnumerable<IUseCase> useCases)
        {
            return OfUseCases(useCases.ToArray());
        }

        private static CommandEnvelope Envelope(ICommand command)
        {
            return new CommandEnvelope(MessageChainInfo.Init(), command);
        }


        public class NoStoryException : Exception
        {
            internal NoStoryException(string commandName)
                : base($"No stories found for command :{commandName} in any of the use cases.")
            {
            }
        }
    }
}