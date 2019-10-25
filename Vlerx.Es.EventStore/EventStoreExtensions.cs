using System.Collections.Generic;
using System.Threading.Tasks;
using EventStore.ClientAPI;

namespace Vlerx.Es.EventStore
{
    public static class EventStoreExtensions
    {
        public static async Task<List<ResolvedEvent>> ReadEvents(
            this IEventStoreConnection connection,
            string streamName
        )
        {
            var result = new List<ResolvedEvent>();
            var position = 0;
            const int sliceSize = 4096;
            while (true)
            {
                var slice = await connection.ReadStreamEventsForwardAsync(
                    streamName, position, sliceSize, false
                );
                result.AddRange(slice.Events);
                if (slice.IsEndOfStream) break;
                position += sliceSize;
            }

            return result;
        }
    }
}