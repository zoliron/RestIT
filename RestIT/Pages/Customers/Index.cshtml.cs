using RestIT.Areas.Identity.Authorization;
using RestIT.Data;
using RestIT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestIT.Pages.Customers
{
    public class IndexModel : DI_BasePageModel
    {
        public IndexModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<ApplicationUser> userManager)
            : base(context, authorizationService, userManager)
        {
        }

        public IList<Customer> Customer { get; set; }

        public async Task OnGetAsync()
        {
            var customers = from c in Context.Customer
                           select c;

            var isAuthorized = User.IsInRole(Constants.CustomerManagersRole) ||
                               User.IsInRole(Constants.CustomerAdministratorsRole);

            var currentUserId = UserManager.GetUserId(User);

            // Only approved customers are shown UNLESS you're authorized to see them
            // or you are the owner.
            if (!isAuthorized)
            {
                customers = customers.Where(c => c.Status == CustomerStatus.Approved
                                            || c.OwnerID == currentUserId);
            }

            Customer = await customers.ToListAsync();
        }
    }
}
