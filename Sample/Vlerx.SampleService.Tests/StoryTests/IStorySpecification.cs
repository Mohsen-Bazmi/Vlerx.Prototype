using Vlerx.Es.Messaging;

namespace Vlerx.SampleService.Tests.StoryTests
{
    public interface IStorySpecification
    {
        string Sut { get; }
        IDomainEvent[] Given { get; }
        ICommand When { get; }
        IDomainEvent[] Then { get; }
    }
}