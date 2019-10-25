using System.Collections.Generic;
using System.Linq;
using Force.DeepCloner;
using Vlerx.Es.Messaging;

namespace Vlerx.Es.DomainModel
{
    public interface IAggregateState<TState>
    {
        string StreamName { get; }
        long Version { get; }
        string StreamNameForId(string id);
        TState ApplySingle(TState state, IDomainEvent @event);
    }

    public abstract class AggregateState<TState>
        : IAggregateState<TState>
        where TState : AggregateState<TState>
    {
        public string Id { get; protected set; }

        public abstract string StreamNameForId(string id);
        public string StreamName => StreamNameForId(Id);
        public long Version { get; protected set; } = -1;

        public TState ApplySingle(TState state, IDomainEvent @event)
        {
            var method = state.GetType()
                .GetMethods()
                .SingleOrDefault(m => m.Name == "On"
                                      && m.GetParameters().First().ParameterType == @event.GetType());
            state.Version++;
            if (null == method)
                //                _log.info()
                //                throw new Exception($"'{state.GetType()}.On({@event.GetType()})' not found.");
                return state;

            method.Invoke(state, new[] {(object) @event});
            // state.AsDynamic().On(@event);
            // ((dynamic)state).On((dynamic) @event);
            return state;
        }

        protected virtual void Validate()
        {
        }

        private TState ApplyAndValidateSingle(TState current, IDomainEvent @event)
        {
            var newState = ApplySingle(current.DeepClone(), @event);
            newState.Validate();
            return newState;
        }

        public Transition Apply(IEnumerable<IDomainEvent> changes)
        {
            return Apply(changes.ToArray());
        }

        public Transition Apply(params IDomainEvent[] changes)
        {
            var currentState = this as TState;
            var newState = changes.Aggregate(currentState, ApplyAndValidateSingle);
            return Transition.To(newState, changes);
        }

        public Transition Apply(
            Transition current,
            params IDomainEvent[] events)
        {
            var newState = events.Aggregate(current.State, ApplyAndValidateSingle);
            return Transition.To(newState, current.Events.ToList().Concat(events));
        }

        public Transition WithNoTransitions()
        {
            return Transition.To(this as TState, new List<IDomainEvent>());
        }


        public class Transition
        {
            public TState State { get; private set; }
            public IEnumerable<IDomainEvent> Events { get; private set; }

            internal static Transition To(TState state, IEnumerable<IDomainEvent> newEvents)
            {
                return new Transition {State = state, Events = newEvents};
            }
        }
    }
}