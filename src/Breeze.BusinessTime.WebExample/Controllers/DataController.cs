using System;
using System.Configuration;
using System.Web.Http;
using Breeze.BusinessTime.Authorization;
using Breeze.BusinessTime.WebExample.Models;
using Breeze.BusinessTime.WebExample.Services;
using Breeze.ContextProvider;
using Breeze.WebApi2;
using Newtonsoft.Json.Linq;

namespace Breeze.BusinessTime.WebExample.Controllers
{
    [Authorize(Roles = "Admin")]
    [BreezeController]
    public class DataController : ApiController, IAmABreezeController
    {
        private readonly AuthorizedDbContextProvider<ApplicationDbContext> _contextProvider;
        private const string adminRole = "Admin";

        public DataController()
        {
            var useAttributeAuthorization = Boolean.Parse(ConfigurationManager.AppSettings["BreezeBusinessTime_UseAttributeAuthorization"]);

            if (useAttributeAuthorization)
            {
                _contextProvider = new AuthorizedDbContextProvider<ApplicationDbContext>(User, adminRole);
            }
            else
            {
                var registry = RegistryAuthorizationProvider.Create()
                    .Register<Car>("Owner, Dealer")
                    .Register<Dealer>("Dealer");

                _contextProvider = new AuthorizedDbContextProvider<ApplicationDbContext>(User, registry, adminRole);
            }
            
            _contextProvider.BeforePipeline.Add(new PreferredDealerProtector(User));
            _contextProvider.AfterPipeline.Add(new BreezeAuditProcessor(User));
        }

        [HttpGet]
        [AllowAnonymous]
        public string Metadata()
        {
            return _contextProvider.Metadata();
        }

        [HttpPost]
        [OverrideAuthorization, Authorize]
        public SaveResult SaveChanges(JObject saveBundle)
        {
            return _contextProvider.SaveChanges(saveBundle);
        }
    }
}
