using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vlerx.Es.Messaging;
using Vlerx.Es.Process;
using Vlerx.Es.StoryBroker;
using Vlerx.InternalMessaging;

// using Serilog;

namespace Vlerx.SampleService.Web.Processes
{
    public abstract class ProcessWithState<TState> : IProcessReactor
        where TState : ProcessState<TState>
        , new()
    {
        private readonly IProcessStateRepository<TState> _repository;

        // readonly ILogger _log;
        private readonly IStories _stories;

        private readonly Dictionary<Type, Func<EventEnvelope, Task>> _storyBook =
            new Dictionary<Type, Func<EventEnvelope, Task>>();


        public ProcessWithState(
            IProcessStateRepository<TState> repository
            , IStories stories
        )
        {
            _repository = repository;
            _stories = stories;
            // _log = Log.ForContext(GetType());
        }
        // readonly ILogger _log;

        public Task ReactTo(EventEnvelope @event)
        {
            return _storyBook[@event.Payload.GetType()](@event);
        }

        public bool CanReactTo(IDomainEvent @event)
        {
            return _storyBook.ContainsKey(@event.GetType());
        }

        protected virtual string GetProcessIdFor(EventEnvelope @event)
        {
            return @event.ChainInfo.CorrelationId;
        }

        private void ContinueStory<TEvent>(Transaction<TEvent> transaction)
            where TEvent : class, IDomainEvent
        {
            AddStory<TEvent>(@event =>
                LoadTellAndSave(@event, GetProcessIdFor(@event), transaction));
        }


        protected virtual TState InitializeState(string processId)
        {
            return new TState().WithId(processId);
        }

        private static IEnumerable<CommandEnvelope> Envelope(IEnumerable<ICommand> commands, EventEnvelope cause)
        {
            return commands.Select(c => new CommandEnvelope(
                new MessageChainInfo(Guid.NewGuid().ToString()
                    , cause.ChainInfo.CorrelationId
                    , cause.ChainInfo.MessageId)
                , c));
        }

        private async Task LoadTellAndSave<TEvent>(
            EventEnvelope @event,
            string processId,
            Transaction<TEvent> transaction)
            where TEvent : class, IDomainEvent
        {
            // _log.Debug("Processing @event {@event}", @event);
            var state = await _repository.Load(processId)
                        ?? InitializeState(processId);
            var payload = (TEvent) @event.Payload;

            transaction.UpdateState(state, payload);
            var commands = transaction.GetCommands(state, payload);
            // var (newState, commands) = update(state, (TEvent)@event.Payload);

            //TODO: Apply UOW if needed
            var envelopedCommands = Envelope(commands, @event);
            Task.WaitAll(envelopedCommands.Select(_stories.ContinueWith).ToArray());
            await _repository.Save(state, state.Version);
        }

        private void AddStory<TEvent>(Func<EventEnvelope, Task> story)
        {
            var eventType = typeof(TEvent);
            if (_storyBook.ContainsKey(eventType))
                throw new DuplicateStoryException(eventType.Name, typeof(TState).Name);
            _storyBook.Add(eventType, story);
        }

        protected Transaction<TEvent> When<TEvent>(Action<TState, TEvent> transitState)
            where TEvent : class, IDomainEvent
        {
            return new Transaction<TEvent>(transitState, ContinueStory);
        }

        protected Transaction<TEvent> When<TEvent>()
            where TEvent : class, IDomainEvent
        {
            return new Transaction<TEvent>((s, e) => { }
                , ContinueStory);
        }

        protected IEnumerable<ICommand> Commands(ICommand command, params ICommand[] commands)
        {
            return commands.Prepend(command);
        }

        public class Transaction<TEvent>
            where TEvent : class, IDomainEvent
        {
            private readonly Action<Transaction<TEvent>> _continueStoryWith;

            public Transaction(Action<TState, TEvent> transitState
                , Action<Transaction<TEvent>> continueStoryWith)
            {
                UpdateState = transitState;
                _continueStoryWith = continueStoryWith;
            }

            public Action<TState, TEvent> UpdateState { get; }

            public Func<TState, TEvent, IEnumerable<ICommand>> GetCommands { get; private set; }
                = (_, n) => new ICommand[0] { };

            public Transaction<TEvent> With(Func<TState, TEvent, IEnumerable<ICommand>> getCommands)
            {
                GetCommands = getCommands;
                return this;
            }

            public void ContinueStory()
            {
                _continueStoryWith(this);
            }
        }

        private class DuplicateStoryException : Exception
        {
            public DuplicateStoryException(string storyName, string stateName)
                : base($"Story : {storyName} already exists in use cases of ${stateName}")
            {
            }
        }
    }
}