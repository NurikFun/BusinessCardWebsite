using BusinessCardWebsite.DB;
using BusinessCardWebsite.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BusinessCardWebsite.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            if (Request.Cookies["auth"] != null)
            {
                var c = new HttpCookie("auth", "");
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
                Session.Remove("UserLogin");
            }

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new UserModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "login,password")] UserModel user)
        {
            AccountDb db = new AccountDb(ConfigurationManager.ConnectionStrings["DB"].ConnectionString);
            user.authed = db.checkUser(user).resulCode;
            if (user.authed)
            {
                Session["UserLogin"] = user.login;
                FormsAuthentication.SetAuthCookie(user.login, true);
                user.session_value = Session.SessionID;
                db.updateSession(user);
                return RedirectToAction("Index", "Request");
            }
            else
            {
                user.message = "Неправильный логин или пароль";
                return View(user);
            }
        }


    }
}