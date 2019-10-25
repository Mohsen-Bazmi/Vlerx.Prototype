using EventStore.ClientAPI;

namespace Vlerx.Es.EventStore
{
    public static class EventStoreConfiguration
    {
        public static IEventStoreConnection ConfigureEsConnection(
            string connectionString,
            string connectionName)
        {
            return EventStoreConnection.Create(connectionString
                , ConnectionSettings.Create().KeepReconnecting()
                , connectionName);
        }
    }
}