using System.Threading.Tasks;

namespace Vlerx.InternalMessaging
{
    public interface IListenTo<in TMessage>
    {
        Task On(TMessage message);
    }
}