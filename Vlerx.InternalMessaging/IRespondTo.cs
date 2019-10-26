using System;

namespace Vlerx.InternalMessaging
{
    public interface IRespondTo<in TMessage, TResponse>
    {
        TResponse Get(TMessage message);
    }
}
