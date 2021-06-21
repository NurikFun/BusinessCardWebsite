using BusinessCardWebsite.DB;
using BusinessCardWebsite.Models;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusinessCardWebsite.Controllers
{
    public class ProjectController : Controller
    {
        private SiteDb db = new SiteDb(ConfigurationManager.ConnectionStrings["DB"].ConnectionString);


        public ActionResult Index(int id = 0)
        {
            ViewBag.directions = db.getDirections();
            return View(db.getProjects(id));
        }

        public ActionResult Details(int id = 0)
        {
            if (id == 0)
            {
                return HttpNotFound();
            }

            Project project = db.getProject(id);

            if (project.name == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        [Authorize]
        public ActionResult Edit(int id = 0)
        {
            if (id == 0)
            {
                return HttpNotFound();
            }

            Project project = db.getProject(id);

            SelectList slDirections = new SelectList(getDirections(), "id", "name");
            ViewBag.directions = slDirections;

            if (project.name == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,direction_id,name,description,content,img_path,doc_path, images")] Project project)
        {
            db.updateProject(project);
            return RedirectToAction("Index", "Project");
        }

        [Authorize]
        public ActionResult Delete(int id = 0)
        {
            if (id == 0)
            {
                return HttpNotFound();
            }

            db.deleteProject(id);
            return RedirectToAction("Index", "Project");
        }

        [Authorize]
        public ActionResult Create()
        {
            SelectList slDirections = new SelectList(getDirections(), "id", "name");
            ViewBag.directions = slDirections;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(Project project)
        {
            db.insertProject(project);
            return RedirectToAction("Index", "Project");
        }

        private List<Direction> getDirections()
        {
            List<Direction> directions = db.getDirections();
            return directions;
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult UploadFile(HttpPostedFileBase uploadedFile)
        {
            var vReturnImagePath = string.Empty;
            if (uploadedFile.ContentLength > 0)
            {
                var supportedTypes = new[] { "pdf", "doc", "docx" };

                string path_server = string.Empty;

                var fileExt = Path.GetExtension(uploadedFile.FileName).Substring(1);
                if (!supportedTypes.Contains(fileExt))
                {
                    path_server = "/Content/Upload/Img/";
                }
                else
                {
                    path_server = "/Content/Upload/Doc/";
                }

                var vFileName = Path.GetFileNameWithoutExtension(uploadedFile.FileName);
                var vExtension = Path.GetExtension(uploadedFile.FileName);

                string sImageName = DateTime.Now.ToString("yyyy_MM_dd_HH_mm__") + vFileName;

                var vImageSavePath = Server.MapPath(path_server) + sImageName + vExtension;

                vReturnImagePath = path_server + sImageName + vExtension;
                ViewBag.Msg = vImageSavePath;
                var path = vImageSavePath;

                uploadedFile.SaveAs(path);
                var vImageLength = new FileInfo(path).Length;

                TempData["message"] = string.Format("File was added successfully");
            }
            return Json(Convert.ToString(vReturnImagePath), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public bool DeleteFile(String fileName)
        {
            removeFileFromServer(fileName);
            return true;
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public bool DeleteFileDb(String fileName)
        {
            removeFileFromServer(fileName);
            db.deleteImage(fileName);
            return true;
        }

        private void removeFileFromServer(String fileName)
        {
            string path = Server.MapPath("~" + fileName);
            FileInfo file = new FileInfo(path);
            if (file.Exists)
            {
                file.Delete();
            }
        }



        [Authorize]
        public FileResult DownloadUploadFolder()
        {
            if (zipFile())
            {
                string file_path = Server.MapPath("~/Content/zip/site_upload_folder.zip");
                string file_type = "application/zip";
                string file_name = "site_upload_folder" + DateTime.Now.ToString("_yyyy_MM_dd_HH_mm") + ".zip";
                return File(file_path, file_type, file_name);
            }
            return null;

        }

        private bool zipFile()
        {
            bool isZip = false;
            try
            {
                using (var zip = new ZipFile())
                {
                    zip.AddEntry("about-file.txt", "This zip has upload folder\nDate create: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm"));
                    zip.AddDirectory(Server.MapPath("~/Content/Upload/"));
                    zip.Save(Server.MapPath("~/Content/zip/site_upload_folder.zip"));
                    isZip = true;
                    return isZip;
                }
            }
            catch (Exception e)
            {
                return isZip;
            }

        }

    }
}