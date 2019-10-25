using System.Threading.Tasks;
using Vlerx.Es.Messaging;

namespace Vlerx.Es.UseCases
{
    public interface IUseCase
    {
        Task Tell(CommandEnvelope command);

        bool CanTell(ICommand command);
    }
}