using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Lib.Models;
using System.Web.Security;

namespace Lib.Controllers
{
    public class AdminsController : Controller
    {
        private ModelContext db = new ModelContext();
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Login(Admin model)
        {
            if (model.AdminName != null && model.Password != null)
            {
                var admin = db.Admins.SingleOrDefault(
                    s => s.AdminName.Equals(model.AdminName));
                if (Membership.Providers["Admin"].ValidateUser(model.AdminName, model.Password) && ModelState.IsValid)
                {
                    FormsAuthentication.SetAuthCookie(model.AdminName, false);
                    return RedirectToAction("IndexAdmin", "Books");
                }
                else
                {
                    ModelState.AddModelError("", "Wrong admin name or password !!!");
                }

            }
            else
            {
                ModelState.AddModelError("", "Please input admin name and password !!!");
            }
            return View(model);
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Admins");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
