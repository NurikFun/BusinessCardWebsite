using BusinessCardWebsite.DB;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusinessCardWebsite.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            SiteDb db = new SiteDb(ConfigurationManager.ConnectionStrings["DB"].ConnectionString);
            return View(db.getDirections());
        }


        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Formulation()
        {
            return View();
        }

        public ActionResult Testing()
        {
            return View();
        }

        public ActionResult Manufacturing()
        {
            return View();
        }
    }
}