using BusinessCardWebsite.DB;
using BusinessCardWebsite.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusinessCardWebsite.Controllers
{
    public class RequestController : Controller
    {
        private SiteDb db = new SiteDb(ConfigurationManager.ConnectionStrings["DB"].ConnectionString);

        public ActionResult RequestForm()
        {
            ViewBag.budgets = getBudgets();
            ViewBag.fields = getFields();
            return PartialView(new Request());
        }

        private SelectList getBudgets()
        {
            List<string> budgets = new List<string>();
            budgets.Add("0 - 2 000 000 тг");
            budgets.Add("2 000 000 - 10 000 000 тг");
            budgets.Add("> 10 000 000 тг");

            SelectList slBudgets = new SelectList(budgets);
            return slBudgets;
        }


        private SelectList getFields()
        {
            List<string> fields = new List<string>();
            fields.Add("Интернет вещей");
            fields.Add("Электроника");
            fields.Add("Другое");

            SelectList slFields = new SelectList(fields);
            return slFields;
        }

        [Authorize]
        public ActionResult Index()
        {
            return View(db.getRequests());
        }

        [Authorize]
        public ActionResult Details(int id = 0)
        {
            if (id == 0)
            {
                return HttpNotFound();
            }

            Request request = db.getRequest(id);
            if (request.name == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        [Authorize]
        public ActionResult Delete(int id = 0)
        {
            if (id == 0)
            {
                return HttpNotFound();
            }

            SiteDb db = new SiteDb(ConfigurationManager.ConnectionStrings["DB"].ConnectionString);
            db.deleteRequest(id);
            return RedirectToAction("Index", "Request");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,email,phone,company_name,estimated_budget,field,description")] Request request)
        {
            if (ModelState.IsValid)
            {
                db.insertRequest(request);
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }



    }
}