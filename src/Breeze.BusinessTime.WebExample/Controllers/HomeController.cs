using System.Collections.Generic;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Breeze.BusinessTime.WebExample.Services;
using Microsoft.Owin.Security;

namespace Breeze.BusinessTime.WebExample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(string name, string role)
        {
            SignIn(name,role);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(AuthenticationTypes.ApplicationCookie);

            return RedirectToAction("Index");
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void SignIn(string name, string role)
        {
            var identity = new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, name),
                    new Claim(ClaimTypes.Role, role)
                }, AuthenticationTypes.ApplicationCookie);

            AuthenticationManager.SignOut(AuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(identity);
        }
    }
}