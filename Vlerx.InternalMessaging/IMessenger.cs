using System.Threading.Tasks;

namespace Vlerx.InternalMessaging
{
    public interface IMessenger<in TMessage>
    {
        Task Dispatch(TMessage message);
    }
}