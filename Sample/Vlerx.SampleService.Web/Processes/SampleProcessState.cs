using System;
using System.Collections.Generic;
using Vlerx.Es.Messaging;
using Vlerx.Es.Process;
using Vlerx.SampleContracts.Customers;

namespace Vlerx.SampleService.Web.Processes
{
    public class SampleProcessState
        : ProcessState<SampleProcessState>
    {
        public int CustomerRegisteredCount { get; private set; }

        public void IncrementCustomerRegisterCount()
        {
            CustomerRegisteredCount++;
            Console.WriteLine($"On CustomerRegistered count: {CustomerRegisteredCount}");
        }


        public IEnumerable<ICommand> On(CustomerRegistered @event)
        {
            IncrementCustomerRegisterCount();
            Console.WriteLine($"On CustomerRegistered count: {CustomerRegisteredCount}");
            return new ICommand[] { };
        }
    }
}