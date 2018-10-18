using RestIT.Areas.Identity.Authorization;
using RestIT.Data;
using RestIT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace RestIT.Pages.Customers
{
    public class DetailsModel : DI_BasePageModel
    {
        public DetailsModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<ApplicationUser> userManager) 
            : base(context, authorizationService, userManager)
        {
        }

        public Customer Customer { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Customer = await Context.Customer.FirstOrDefaultAsync(m => m.ID == id);

            if (Customer == null)
            {
                return NotFound();
            }

            var isAuthorized = User.IsInRole(Constants.CustomerManagersRole) ||
                                          User.IsInRole(Constants.CustomerAdministratorsRole);

            var currentUserId = UserManager.GetUserId(User);

            if (!isAuthorized
                && currentUserId != Customer.OwnerID
                && Customer.Status != CustomerStatus.Approved)
            {
                return new ChallengeResult();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id, CustomerStatus status)
        {
             var customer = await Context.Customer.FirstOrDefaultAsync(
                                                       m => m.ID == id);

            if (customer == null)
            {
                return NotFound();
            }

            var customerOperation = (status == CustomerStatus.Approved) 
                                                       ? CustomerOperations.Approve
                                                       : CustomerOperations.Reject;

            var isAuthorized = await AuthorizationService.AuthorizeAsync(User, customer,
                                        customerOperation);
            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }
            customer.Status = status;
            Context.Customer.Update(customer);
            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
