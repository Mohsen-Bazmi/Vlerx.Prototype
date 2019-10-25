using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Vlerx.Es.Messaging;

namespace Vlerx.Es.Bdd.Tools
{
    public class EventsDidNotHappenException : Exception
    {
        public EventsDidNotHappenException(
            IEnumerable<IDomainEvent> expected
            , IEnumerable<IDomainEvent> actual)
            : base(
                $@"Expected  {expected.Count()} events, {actual.Count()} happened.
                            Expected {JsonConvert.SerializeObject(expected.Select(e => e.GetType().Name))}
                            , but {JsonConvert.SerializeObject(actual.Select(e => e.GetType().Name))} happened.
                            Expected {JsonConvert.SerializeObject(expected.Select(e => e.GetType().Name + " : " + JsonConvert.SerializeObject(e))).Replace("\\", "")} events to happen 
                            , but {JsonConvert.SerializeObject(actual.Select(e => e.GetType().Name + " : " + JsonConvert.SerializeObject(e))).Replace("\\", "")} events happened.")
        {
            Expected = expected;
            Actual = actual;
        }

        public IEnumerable<IDomainEvent> Expected { get; }
        public IEnumerable<IDomainEvent> Actual { get; }
    }
}