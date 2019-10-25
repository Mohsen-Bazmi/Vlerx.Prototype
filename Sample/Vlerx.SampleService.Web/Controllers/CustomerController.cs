using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Vlerx.InternalMessaging;
using Vlerx.SampleService.Customers.Commands;
using Vlerx.SampleService.Web.Customers;

namespace Vlerx.SampleService.Web.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IStories _stories;

        public CustomerController(IStories stories)

        {
            _stories = stories;
        }

        public IActionResult Register()
        {
            return View(new RegistrationViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationViewModel viewModel)
        {
            var customerId = Guid.NewGuid().ToString();
            await _stories.Tell(new RegisterCustomer(
                customerId
                , viewModel.FirstName
                , viewModel.LastName
                , viewModel.Address
                , viewModel.PhoneNumber
            ));
            return RedirectToAction(nameof(Relocate), new {customerId});
        }

        [HttpGet("Customer/Relocate/customerId")]
        public IActionResult Relocate(string customerId)
        {
            if (null == customerId)
                throw new ArgumentNullException(nameof(customerId));

            return View(new RelocationViewModel
            {
                CustomerId = customerId
            });
        }

        [HttpPost]
        public async Task<IActionResult> Relocate(RelocationViewModel viewModel)
        {
            await _stories.Tell(new RelocateCustomer(
                viewModel.CustomerId
                , viewModel.NewAddress
                , viewModel.NewPhoneNumber
            ));
            return RedirectToAction(nameof(Register));
        }
    }
}