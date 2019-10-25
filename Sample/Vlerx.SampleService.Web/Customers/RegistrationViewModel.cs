using System.ComponentModel.DataAnnotations;

namespace Vlerx.SampleService.Web.Customers
{
    public class RegistrationViewModel
    {
        [Display(Name = "First Name")] public string FirstName { get; set; }
        [Display(Name = "Last Name")] public string LastName { get; set; }
        [Display(Name = "Address")] public string Address { get; set; }
        [Display(Name = "Phone Number")] public string PhoneNumber { get; set; }
    }
}