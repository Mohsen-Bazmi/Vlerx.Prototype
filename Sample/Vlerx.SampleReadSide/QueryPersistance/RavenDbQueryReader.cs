using System;
using System.Collections.Generic;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace Vlerx.SampleReadSide.QueryPersistance
{
    public class RavenDbQueryReader<TViewModel>
                     : IQueryReader<TViewModel>
    {
        readonly IDocumentStore _store;
        public RavenDbQueryReader(IDocumentStore store)
        => _store = store;

        public IEnumerable<TViewModel> Get(All _)
            => _store.OpenSession()
                     .Query<TViewModel>();
    }
}