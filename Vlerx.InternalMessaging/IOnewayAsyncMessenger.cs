using System;
using System.Threading.Tasks;

namespace Vlerx.InternalMessaging
{
    public interface IOnewayAsyncMessenger<in TMessage>
    {
        Task Dispatch(TMessage message);
    }
}
