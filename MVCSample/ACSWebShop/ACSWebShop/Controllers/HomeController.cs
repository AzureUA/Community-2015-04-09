using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace ACSWebShop.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";                       
                    
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Claims() 
        { 
            ViewBag.Message = "Your claims page.";

            ViewBag.ClaimsIdentity = Thread.CurrentPrincipal.Identity;

            return View();
        }
    }
}