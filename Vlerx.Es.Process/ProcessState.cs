using System;
using System.Collections.Generic;
using Force.DeepCloner;
using Vlerx.Es.Messaging;

namespace Vlerx.Es.Process
{
    public abstract class ProcessState<TState>
        : IProcessState<TState>
        where TState : ProcessState<TState>
    {
        public long Version { get; protected set; } = -1;
        public string Id { get; protected set; }


        public TState WithId(string id)
        {
            Id = id;
            return this as TState;
        }

        public Transition Apply<TEvent>(TEvent change)
        {
            var currentState = this as TState;
            var newState = currentState.DeepClone();
            try
            {
                IEnumerable<ICommand> cmds = ((dynamic) newState).On((dynamic) change);
                Version++;
                return new Transition(newState, cmds);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Transition WithNoTransitions()
        {
            return new Transition(this as TState, new List<ICommand>());
        }


        public class Transition
        {
            internal Transition(TState state, IEnumerable<ICommand> commands)
            {
                State = state;
                Commands = commands;
            }

            public TState State { get; }
            public IEnumerable<ICommand> Commands { get; }
        }
    }
}