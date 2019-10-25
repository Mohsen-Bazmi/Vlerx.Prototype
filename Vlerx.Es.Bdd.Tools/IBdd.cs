using Vlerx.Es.Messaging;

namespace Vlerx.Es.Bdd.Tools
{
    public interface IBdd
    {
        void Given(params IDomainEvent[] events);

        void When(ICommand command);

        void Then(params IDomainEvent[] expectedEvents);
    }
}