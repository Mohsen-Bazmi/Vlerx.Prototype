using Vlerx.Es.Process;
using Vlerx.Es.StoryBroker;
using Vlerx.InternalMessaging;
using Vlerx.SampleContracts.Customers;

namespace Vlerx.SampleService.Web.Processes
{
    public class SampleProcess : ProcessWithState<SampleProcessState>
    {
        public SampleProcess(IProcessStateRepository<SampleProcessState> repository
            , IStories stories)
            : base(repository, stories)
        {
            When<CustomerRegistered>((state, e) => state.IncrementCustomerRegisterCount())
                .ContinueStory();
            // When<CustomerRegistered>((state, e) => state.IncrementCustomerRegisterCount())
            //    .With((state, e) => Commands(new RegisterCustomer()))
            //    .ContinueStory();

            // When<CustomerRegistered>()
            //     .ContinueStory();
        }
    }
}