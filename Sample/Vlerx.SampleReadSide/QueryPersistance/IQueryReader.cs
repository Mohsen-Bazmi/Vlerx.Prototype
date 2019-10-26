using System.Collections.Generic;
using Vlerx.InternalMessaging;
public interface All
{
}
public static class Queries
{
    public static All All = default(All);
}
public interface IQueryReader<TViewModel>
    : IRespondTo<All, IEnumerable<TViewModel>>
{

}