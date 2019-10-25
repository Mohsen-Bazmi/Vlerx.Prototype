using System.ComponentModel.DataAnnotations;

namespace Vlerx.SampleService.Web.Customers
{
    public class RelocationViewModel
    {
        public string CustomerId { get; set; }
        [Display(Name = "Address")] public string NewAddress { get; set; }
        [Display(Name = "Phone Number")] public string NewPhoneNumber { get; set; }
    }
}