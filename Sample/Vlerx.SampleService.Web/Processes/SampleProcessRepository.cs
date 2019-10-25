using System;
using System.Threading.Tasks;
using Vlerx.Es.Process;

namespace Vlerx.SampleService.Web.Processes
{
    public class SampleStateRepository : IProcessStateRepository<SampleProcessState>
    {
        private SampleProcessState State;
        // public List<SampleProcessState> States = new List<SampleProcessState>();

        public Task<SampleProcessState> Load(string id)
        {
            Console.WriteLine($"Loading process with Id : {id} state is null? : {State == null}");
            return Task.FromResult(State);
            // return Task.FromResult(States.SingleOrDefault(s => s.Id == id));
        }

        public Task Save(SampleProcessState state, long version)
        {
            Console.WriteLine($"Saving process state with Id : {state.Id}");
            State = state;
            // States.Add(state);
            return Task.CompletedTask;
        }
    }
}