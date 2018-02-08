using ClaimsBasedAuthentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace ClaimsBasedAuthentication.Controllers
{
    public class HomeController : Controller
    {
        private object newIdentity;

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ClaimsIdentityCollection ValidateToken(SecurityToken token)
        {
            ClaimsIdentityCollection claimsIdentityCollection = new ClaimsIdentityCollection();
            if (token is MyCustomToken)
            {
                MyCustomToken mycustomtoken = token as MyCustomToken;
                if (mycustomtoken.ValidateThisSignature())
                {
                    ClaimsIdentity newIdentity = new ClaimsIdentity((token as
                    MyCustomToken).Claims);
                }
            }
            claimsIdentityCollection.Add(newIdentity);
            return claimsIdentityCollection;
        }
    }
}
