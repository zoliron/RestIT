using RestIT.Areas.Identity.Authorization;
using RestIT.Data;
using RestIT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RestIT.Pages.Customers
{
    public class IndexModel : DI_BasePageModel
    {
        public IndexModel(ApplicationDbContext context, IAuthorizationService authorizationService, UserManager<Customer> userManager) : base(context, authorizationService, userManager){ }

        public IList<Customer> Customers;
        public SelectList Types;
        public SelectList Citys;
        public SelectList Genders;
        public string CustomerFavType { get; set; }
        public string CustomerCity { get; set; }
        public string SearchString { get; set; }
        public string CustomerGender { get; set; }

        public async Task OnGetAsync(string searchString, string CustomerFavType, string CustomerCity, string CustomerGender)
        {
            IQueryable<CustomerRestType> favTypeQuery = from m in Context.Customer
                                                        orderby m.custRestType
                                                        select m.custRestType;

            IQueryable<CustomerCity> cityQuery = from m in Context.Customer
                                                 orderby m.custCity
                                                 select m.custCity;

            IQueryable<CustomerSex> genderQuery = from m in Context.Customer
                                                  orderby m.custSex
                                                  select m.custSex;

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

            if (!String.IsNullOrEmpty(searchString))
            {
                customers = customers.Where(s => s.custName.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(CustomerFavType))
            {
                customers = customers.Where(x => x.custRestType.ToString() == CustomerFavType);
            }

            if (!String.IsNullOrEmpty(CustomerCity))
            {
                customers = customers.Where(x => x.custCity.ToString() == (CustomerCity));
            }

            if (!String.IsNullOrEmpty(CustomerGender))
            {
                customers = customers.Where(x => x.custSex.ToString() == (CustomerGender));
            }

            Customers = await customers.ToListAsync();
            SearchString = searchString;
            Types = new SelectList(await favTypeQuery.Distinct().ToListAsync());
            Citys = new SelectList(await cityQuery.Distinct().ToListAsync());
            Genders = new SelectList(await genderQuery.Distinct().ToListAsync());

            //Customer = await customers.ToListAsync();
            //SearchString = searchString;
        }
    }
}
