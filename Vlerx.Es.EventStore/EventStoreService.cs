using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Microsoft.Extensions.Hosting;
using Serilog;
using Vlerx.Es.EventStore.Subscription;

namespace Vlerx.Es.EventStore
{
    public class EventStoreService : IHostedService
    {
        private readonly IEventStoreConnection _esConnection;
        private readonly SubscriptionIntegrator[] _subscriptionManager;
        private readonly ConnectionSupervisor _supervisor;

        public EventStoreService(
            IEventStoreConnection esConnection,
            params SubscriptionIntegrator[] subscriptionManagers)
        {
            _esConnection = esConnection;
            _subscriptionManager = subscriptionManagers;
            _supervisor = new ConnectionSupervisor(
                esConnection,
                () => Log.Fatal("Fatal failure with EventStore connection"));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _supervisor.Initialize();
            await _esConnection.ConnectAsync();

            await Task.WhenAll(
                _subscriptionManager
                    .Select(projection => projection.Start())
            );
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _supervisor.Shutdown();
            _esConnection.Close();
            return Task.CompletedTask;
        }
    }
}