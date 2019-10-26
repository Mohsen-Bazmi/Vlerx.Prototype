using System.Collections.Generic;
using Vlerx.InternalMessaging;

namespace Vlerx.SampleReadSide.QueryPersistance
{
    public interface All
    {
    }
    public static class Query
    {
        public static All All = default(All);
    }
    public interface IQueryReader<TViewModel>
        : IRespondTo<All, IEnumerable<TViewModel>>
    {

    }
}