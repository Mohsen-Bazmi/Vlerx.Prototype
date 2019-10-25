using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vlerx.Es.DataStorage;
using Vlerx.Es.Persistence;

namespace Vlerx.Es.Bdd.Tools
{
    public class InMemoryEventStorage : IEventStorage
    {
        private readonly List<UntypedEventEnvelope> db = new List<UntypedEventEnvelope>();
//        IDictionary<string, UntypedEventEnvelope[]> db = new Dictionary<string, UntypedEventEnvelope[]>();

        public Task AppendEvents(string streamName, long version, UntypedEventEnvelope[] changes)
        {
            db.AddRange(changes);
//            db.Add(streamName, changes);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<UntypedEventEnvelope>> ReadEvents(string streamName)
        {
//            var exists = db.TryGetValue(streamName, out var events);
//            if (!exists)
//                throw new StreamNotFoundException(streamName);
            return Task.FromResult(db.AsEnumerable());
        }

        public Task<bool> Exists(string streamName)
        {
            throw new NotImplementedException();
        }

        public class StreamNotFoundException : Exception
        {
            public StreamNotFoundException(string streamName)
            {
                StreamName = streamName;
            }

            public string StreamName { get; }
        }
    }
}