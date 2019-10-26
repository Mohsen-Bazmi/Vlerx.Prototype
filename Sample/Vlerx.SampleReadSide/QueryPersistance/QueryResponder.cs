using System.Collections.Generic;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

public class RavenDbQueryReader<TViewModel> :
    IQueryReader<TViewModel>
{
    readonly IDocumentSession _session;
    public RavenDbQueryReader(IDocumentStore store)
    => _session = store.OpenSession();
    public IEnumerable<TViewModel> Get(All _)
    => _session.Query<TViewModel>();
}