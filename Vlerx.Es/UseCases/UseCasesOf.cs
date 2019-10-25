using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vlerx.Es.DomainModel;
using Vlerx.Es.Messaging;
using Vlerx.Es.Persistence;

// using Serilog;

namespace Vlerx.Es.UseCases
{
    public abstract class UseCasesOf<TState> : IUseCase
        where TState : AggregateState<TState>
    // , new()
    {
        private readonly IRepository<TState> _repository;

        private readonly Dictionary<Type, Func<object, Task>> _storyBook =
            new Dictionary<Type, Func<object, Task>>();
        // readonly ILogger _log;

        protected UseCasesOf(IRepository<TState> repository)
        {
            _repository = repository;
            // _log = Log.ForContext(GetType());
        }

        public Task Tell(CommandEnvelope command)
        {
            var storyOf = _storyBook[command.Payload.GetType()];
            return storyOf(command);
        }

        public bool CanTell(ICommand command)
        {
            return _storyBook.Any(story => story.Key == command.GetType());
        }

        protected void StoryOf<TCommand>(
            Func<TState, TCommand, AggregateState<TState>.Transition> update)
            where TCommand : class, ICommand
        {
            AddStory<TCommand>(cmd =>
            {
                var command = cmd as CommandEnvelope;
                return LoadTellAndSave(command, GetId(command.Payload), update);
            });
        }

        protected void StoryOf<TCommand>(
            Func<TCommand, AggregateState<TState>.Transition> update)
            where TCommand : class
        {
            AddStory<TCommand>(
                cmd =>
                {
                    var command = cmd as CommandEnvelope;
                    return TellAndSaveWithoutLoading(command, update);
                });
        }

        protected abstract TState InitializeState(string id);
        // => new TState();

        protected abstract string GetId(ICommand command);

        private async Task LoadTellAndSave<TCommand>(
            CommandEnvelope command,
            string id,
            Func<TState, TCommand, AggregateState<TState>.Transition> update)
        {
            // _log.Debug("Processing command {command}", command);
            var uow = UnitOfWork<TState>.For(command.ChainInfo.MessageId
                , command.ChainInfo.CorrelationId
                , command.ChainInfo.CausationId
                , InitializeState(id));
            var state = await _repository.Load(uow, id);
            var transition = update(state, (TCommand) command.Payload);
            var transaction = uow.Track(transition.Events, transition.State);

            var finalVersion = transition.State.Version;
            var originlVersion = finalVersion - transition.Events.Count();
            var expectedVersion = originlVersion;

            await _repository.Save(transaction, expectedVersion);
        }

        private async Task TellAndSaveWithoutLoading<TCommand>(
            CommandEnvelope command,
            Func<TCommand, AggregateState<TState>.Transition> update)
        {
            // _log.Debug("Processing command {command}", command);
            var transition = update((TCommand) command.Payload);
            var uow = UnitOfWork<TState>.For(command.ChainInfo.MessageId
                , command.ChainInfo.CorrelationId
                , command.ChainInfo.CausationId
                , InitializeState(transition.State.Id));
            // var state = uow.State;
            var transaction = uow.Track(transition.Events, transition.State);

            var finalVersion = transition.State.Version;
            var originlVersion = finalVersion - transition.Events.Count();
            var expectedVersion = originlVersion;

            await _repository.Save(transaction, expectedVersion);
        }

        private void AddStory<TCommand>(Func<object, Task> story)
        {
            var commandType = typeof(TCommand);
            if (_storyBook.ContainsKey(commandType))
                throw new DuplicateStoryException(commandType.Name, typeof(TState).Name);
            _storyBook.Add(commandType, story);
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
// protected void StoryOf<TCommand>(
//     Func<TCommand, string> getId,
//     Func<TCommand, AggregateState<TState>.Transition> update)
// where TCommand : class
// => AddStory(typeof(TCommand),
//                  cmd =>
//                  {
//                      var command = cmd as CommandEnvelope<TCommand>;
//                      return LoadExecuteAndSave(command, getId(command.Payload), update);
//                  });


// async Task LoadExecuteAndSave<TCommand>(
//             CommandEnvelope<TCommand> command,
//             string id,
//             Func<TCommand, AggregateState<TState>.Transition> update)

// {
//     try
//     {

//         // _log.Debug("Processing command {command}", command);
//         var uow = UnitOfWork<TState>.For(command.ChainInfo.MessageId
//                                         , command.ChainInfo.CorrelationId
//                                         , command.ChainInfo.CausationId
//                                         , InitializeState());

//         var state = await _repository.Load(uow, id);
//         var transition = update(command.Payload);
//         var transaction = uow.Track(transition.Events, transition.State);
//         await _repository.Save(transaction, state.Version);
//     }
//     catch (Exception e)
//     {
//         // _log.Error(e, "Error occured while handling the command");
//         throw;
//     }
// }

// public Task Tell<TCommand>(TCommand command)
// {
//     var storyOf = _storyBook[typeof(TCommand)];
//     return storyOf(new CommandEnvelope<TCommand>(MessageChainInfo.Init(), command));
// }