using RestIT.Areas.Identity.Authorization;
using RestIT.Data;
using RestIT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace RestIT.Pages.Customers
{
    public class DeleteModel : DI_BasePageModel
    {
        public DeleteModel(
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
                                                     CustomerOperations.Delete);
            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            Customer = await Context.Users.FindAsync(id);

            var customer = await Context
                .Users.AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                     User, customer,
                                                     CustomerOperations.Delete);
            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }

            Context.Users.Remove(Customer);
            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
