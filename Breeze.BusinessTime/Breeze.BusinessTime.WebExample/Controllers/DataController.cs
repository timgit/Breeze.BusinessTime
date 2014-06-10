using System.Web.Http;
using Breeze.BusinessTime.Authorization;
using Breeze.BusinessTime.WebExample.Models;
using Breeze.ContextProvider;
using Breeze.WebApi2;
using Newtonsoft.Json.Linq;

namespace Breeze.BusinessTime.WebExample.Controllers
{
    // Firewall pattern:  Use OverrideAuthorization to grant actions
    [Authorize(Roles = "Admin")]
    [BreezeController]
    public class DataController : ApiController, IAmABreezeController
    {
        private const string AllAllowedRoles = "Owner, Dealer, Admin";

        protected readonly AuthorizedDbContextProvider<ApplicationDbContext> ContextProvider;

        public DataController()
        {
            var roleProvider = new AttributeAuthorizationProvider<AuthorizeEntityAttribute>();

            ContextProvider = new AuthorizedDbContextProvider<ApplicationDbContext>(User, roleProvider, allowedRole: "Admin");
        }

        [HttpGet]
        [AllowAnonymous]
        public string Metadata()
        {
            return ContextProvider.Metadata();
        }

        [HttpPost]
        [OverrideAuthorization, Authorize(Roles = AllAllowedRoles)]
        public SaveResult SaveChanges(JObject saveBundle)
        {
            return ContextProvider.SaveChanges(saveBundle);
        }
    }
}
