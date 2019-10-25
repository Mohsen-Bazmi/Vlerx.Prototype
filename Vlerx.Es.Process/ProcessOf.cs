// using Serilog;

using System;
using System.Linq;
using System.Threading.Tasks;
using Vlerx.Es.Messaging;
using Vlerx.Es.StoryBroker;
using Vlerx.InternalMessaging;

namespace Vlerx.Es.Process
{
    public class ProcessOf<TState> : IProcessReactor
        where TState : ProcessState<TState>
        , new()
    {
        private readonly IProcessStateRepository<TState> _repository;

        // readonly ILogger _log;
        private readonly IStories _stories;

        public ProcessOf(
            IProcessStateRepository<TState> repository
            , IStories stories
        )
        {
            _repository = repository;
            _stories = stories;
            // _log = Log.ForContext(GetType());
        }

        public bool CanReactTo(IDomainEvent @event)
        {
            return typeof(TState)
                .GetMethods()
                // System.Reflection.BindingFlags.NonPublic
                //            | System.Reflection.BindingFlags.Instance)
                .Any(m => m.Name == "On"
                          && m.GetParameters()
                              .Any(p => p.ParameterType == @event.GetType()));
        }

        public async Task ReactTo(EventEnvelope @event)
        {
            // _log.Debug("Processing @event {@event}", @event);
            var processId = GetProcessIdFor(@event);
            var state = await _repository.Load(processId)
                        ?? InitializeState(processId);


            var transition = state.Apply(@event.Payload);

            //TODO: Apply UOW if needed
            var envelopedCommands = transition.Commands.Select(
                c =>
                    new CommandEnvelope(
                        new MessageChainInfo(Guid.NewGuid().ToString()
                            , @event.ChainInfo.CorrelationId
                            , @event.ChainInfo.MessageId)
                        , c));
            Task.WaitAll(envelopedCommands.Select(_stories.ContinueWith).ToArray());
            await _repository.Save(transition.State, state.Version);
        }

        protected virtual string GetProcessIdFor(EventEnvelope @event)
        {
            return @event.ChainInfo.CorrelationId;
        }

        protected virtual TState InitializeState(string processId)
        {
            return new TState().WithId(processId);
        }
    }
}