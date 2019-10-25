using Vlerx.Es.EventMapping.Contracts;
using Vlerx.Es.EventMapping.Json;
using Vlerx.SampleContracts.Customers;

namespace Vlerx.SampleService.Web
{
    public class ContextEventSchema : JsonEventSchemaContext
    {
        public override void OnModelCreating(IContextSchemaConfigs configs)
        {
            AddCustomerEvents(configs);
        }

        //Can be moved to it's own folder:
        private void AddCustomerEvents(IContextSchemaConfigs configs)
        {
            //To avoid the renaming of types from affecting the event reading behaviour:
            configs.Add(new CustomerRegisteredSchemaConfig());
            //OR:
            configs.Add(new SchemaFor<CustomerRelocated>("CustomerRelocated"));
            configs.Add(new SchemaFor<CustomerContactInfoChanged>("CustomerContactInfoChanged"));
        }
    }
}