using Vlerx.Es.EventMapping.Contracts;
using Vlerx.SampleContracts.Customers;

namespace Vlerx.SampleService.Web
{
    public class CustomerRegisteredSchemaConfig : IEventSchema<CustomerRegistered>
    {
        public void Map(ISchemaBuilder<CustomerRegistered> map)
        {
            map.Name("CustomerRegistered");
        }
    }
}