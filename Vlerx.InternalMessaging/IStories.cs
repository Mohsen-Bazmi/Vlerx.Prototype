using System.Threading.Tasks;
using Vlerx.Es.Messaging;

namespace Vlerx.InternalMessaging
{
    public interface IStories
    {
        Task ContinueWith(CommandEnvelope command);

        Task Tell<TCommand>(TCommand command)
            where TCommand : ICommand;
    }
}