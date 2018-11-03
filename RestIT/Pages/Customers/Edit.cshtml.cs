using RestIT.Areas.Identity.Authorization;
using RestIT.Data;
using RestIT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace RestIT.Pages.Customers
{
    public class EditModel : DI_BasePageModel
    {
        public EditModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<Customer> userManager)
            : base(context, authorizationService, userManager)
        {
        }

        [BindProperty]
        public Customer Customer { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            Customer = await Context.Users.FirstOrDefaultAsync(
                                                 m => m.Id == id);

            if (Customer == null)
            {
                return NotFound();
            }

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                      User, Customer,
                                                      CustomerOperations.Update);
            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Fetch Customer from DB to get OwnerID.
            var customer = await Context
                .Customer.AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                     User, customer,
                                                     CustomerOperations.Update);
            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }

            Customer.OwnerID = customer.OwnerID;

            Context.Attach(Customer).State = EntityState.Modified;

            if (customer.Status == CustomerStatus.Approved)
            {
                // If the customer is updated after approval, 
                // and the user cannot approve,
                // set the status back to submitted so the update can be
                // checked and approved.
                var canApprove = await AuthorizationService.AuthorizeAsync(User,
                                        customer,
                                        CustomerOperations.Approve);

                if (!canApprove.Succeeded)
                {
                    customer.Status = CustomerStatus.Submitted;
                }
            }

            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private bool ContactExists(string id)
        {
            return Context.Users.Any(e => e.Id == id);
        }
    }
}
