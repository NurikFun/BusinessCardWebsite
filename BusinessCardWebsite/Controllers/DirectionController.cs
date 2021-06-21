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
    public class DirectionController : Controller
    {
        private SiteDb db = new SiteDb(ConfigurationManager.ConnectionStrings["DB"].ConnectionString);

        public ActionResult LinkListDirections()
        {
            return PartialView(db.getDirections());
        }


        public ActionResult Details(int id = 0)
        {
            if (id == 0)
            {
                return HttpNotFound();
            }

            Direction direction = db.getDirection(id);

            if (direction.name == null)
            {
                return HttpNotFound();
            }
            return View(direction);
        }

        [Authorize]
        public ActionResult Edit(int id = 0)
        {
            if (id == 0)
            {
                return HttpNotFound();
            }

            Direction direction = db.getDirection(id);

            if (direction.name == null)
            {
                return HttpNotFound();
            }
            return View(direction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,direction_id,name,description,content,img_path,doc_path")] Direction direction)
        {
            db.updateDirection(direction);
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult Delete(int id = 0)
        {
            if (id == 0)
            {
                return HttpNotFound();
            }

            db.deleteDirection(id);
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(Direction direction)
        {
            db.insertDirection(direction);
            return RedirectToAction("Index", "Home");
        }

    }
}