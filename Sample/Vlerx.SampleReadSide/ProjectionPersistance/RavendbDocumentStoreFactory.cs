using Raven.Client.Documents;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

namespace Vlerx.SampleReadSide.ProjectionPersistance
{
    public static class RavendbDocumentStoreFactory
    {
        public static IDocumentStore CreateDocStore(string serverUrl, string databaseName)
        {
            var store = new DocumentStore
            {
                Urls = new[] { serverUrl },
                Database = databaseName
            };
            store.Initialize();

            var record = store.Maintenance.Server.Send(
                new GetDatabaseRecordOperation(store.Database)
            );

            if (record == null)
                store.Maintenance.Server.Send(
                    new CreateDatabaseOperation(
                        new DatabaseRecord(store.Database)
                    )
                );

            return store;
        }
    }
}
