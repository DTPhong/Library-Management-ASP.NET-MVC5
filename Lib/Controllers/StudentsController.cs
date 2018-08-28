using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Lib.Models;
using Lib.Code;
using System.Web.Security;

namespace Lib.Controllers
{
    
    public class StudentsController : Controller
    {
        private ModelContext db = new ModelContext();

        // GET: Students
        public ActionResult Index()
        {
            var students = db.Students.Include(s => s.Major);
            return View(students.ToList());
        }

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return PartialView("../Students/_Details", student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            ViewBag.MajorId = new SelectList(db.Majors, "MajorId", "MajorName");
            return PartialView("../Students/_Create");
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentId,StudentName,Password,Address,Phone,MajorId")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MajorId = new SelectList(db.Majors, "MajorId", "MajorName", student.MajorId);
            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.MajorId = new SelectList(db.Majors, "MajorId", "MajorName", student.MajorId);
            return PartialView("../Students/_Edit", student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentId,StudentName,Password,Address,Phone,MajorId")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MajorId = new SelectList(db.Majors, "MajorId", "MajorName", student.MajorId);
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return PartialView("../Students/_Delete", student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Login(Student model)
        {
            if (model.StudentName!=null && model.Password!= null)
            {
                var student = db.Students.SingleOrDefault(
                    s => s.StudentName.Equals(model.StudentName));
                Session["name"] = model.StudentName.ToString();
                Session["id"] = student.StudentId;
                if (Membership.ValidateUser(model.StudentName, model.Password) && ModelState.IsValid)
                {
                    FormsAuthentication.SetAuthCookie(model.StudentName, false);
                    return RedirectToAction("Index", "Books");
                }
                else
                {
                    ModelState.AddModelError("", "Wrong student name or password !!!");
                }

            }
            else
            {
                ModelState.AddModelError("", "Please input student name and password !!!");
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Students");
        }

        public ViewResult Notfound()
        {
            Response.StatusCode = 404;
            return View("NotFound");
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
