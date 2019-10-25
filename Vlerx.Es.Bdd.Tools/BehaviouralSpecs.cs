using Vlerx.Es.Messaging;

namespace Vlerx.Es.Bdd.Tools
{
    public abstract class BehaviouralSpecs : IBdd
    {
        private readonly IBdd _scenarios;

        protected BehaviouralSpecs(IBdd scenarios)
        {
            _scenarios = scenarios;
        }

        public void Given(params IDomainEvent[] events)
        {
            _scenarios.Given(events);
        }

        public void When(ICommand command)
        {
            _scenarios.When(command);
        }

        public void Then(params IDomainEvent[] expectedEvents)
        {
            _scenarios.Then(expectedEvents);
        }
    }
}