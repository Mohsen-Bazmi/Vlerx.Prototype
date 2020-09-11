// namespace Vlerx.SampleService.Tests.StoryTests
// {
//     public  static partial class Examples
//     {

//     }
//     public static partial class Examples
//     {
//         public static IDomainEvent[] Given
//         => new IDomainEvent[]{
//                     new CustomerRegistered("customerId"
//                                             , "Mohsen"
//                                             , "Bazmi"
//                                             , "My Old Address"
//                                             , "09100000000"),
//         };

//         public static ICommand When
//         => new RelocateCustomer("customerId", "newAddress", "newPhoneNumber");

//         public static IDomainEvent[] Then
//         => new IDomainEvent[]{
//                     new CustomerRelocated("customerId", "newAddress"),
//                     new CustomerContactInfoChanged("customerId", "newPhoneNumber")
//         };
//     }
//     public class CurstomerRelocation_ContactInfoChangedRunner
//      {   public string SystemUnderTestName { get; set; } = nameof(Customer);
// // Speculation changes that break me
//         [Fact]
//         public void Test()
//         {
            
//             var storage = new InMemoryEventStorage();
//             var repository = new Repository<Customer.State>(storage);
//             var sut = new CustomerUseCases(repository);

//             BddTool.Test(this, sut);
//         }
//     }
// }