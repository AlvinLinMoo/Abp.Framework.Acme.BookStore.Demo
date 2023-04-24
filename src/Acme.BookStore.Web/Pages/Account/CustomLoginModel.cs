using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Volo.Abp.Account.Web;
using Volo.Abp.Account.Web.Pages.Account;

namespace Acme.BookStore.Web.Pages.Account
{
    public class CustomLoginModel : LoginModel
    {
        public CustomLoginModel(IAuthenticationSchemeProvider schemeProvider, IOptions<AbpAccountOptions> accountOptions, IOptions<IdentityOptions> identityOptions) : base(schemeProvider, accountOptions, identityOptions)
        {
        }

        // Overwrtie your Post or Get Here


        // Post Handler: SSO
        public async Task<IActionResult> OnPostSSOLoginAsync()
        {
            Alerts.Warning("Advantech SSO Login is not implemented yet!");

            return Page();
        }

    }
}
