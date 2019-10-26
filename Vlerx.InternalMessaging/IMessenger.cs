using System.Threading.Tasks;

namespace Vlerx.InternalMessaging
{
    public interface IRequester<in TMessage>
    {
        TResponse Get<TResponse>(TMessage message);
    }
}