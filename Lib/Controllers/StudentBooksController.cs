using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Lib.Models;

namespace Lib.Controllers
{
    public class StudentBooksController : Controller
    {
        private ModelContext db = new ModelContext();

        // GET: StudentBooks
        //public ActionResult Index(string sName)
        //{
        //    int id = Convert.ToInt32(Session["id"]);
        //    var studentBooks = db.StudentBooks.Include(s => s.Book).Include(s => s.Student);
        //    studentBooks = studentBooks.Where(b => b.StudentId==id);
        //    return View(studentBooks.ToList());
        //}
        public ActionResult Index(string startDate, string endDate, string sName)
        {
            int id = Convert.ToInt32(Session["id"]);
            var studentBooks = db.StudentBooks.Include(s => s.Book).Include(s => s.Student);
            studentBooks = studentBooks.Where(b => b.StudentId == id);
            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                var begin = Convert.ToDateTime(startDate);
                var end = Convert.ToDateTime(endDate);
                studentBooks = studentBooks.Where(d => d.IssueDate >= begin && d.IssueDate <= end);
            }
            else if (!string.IsNullOrEmpty(startDate))
            {
                var begin = Convert.ToDateTime(startDate);
                studentBooks = studentBooks.Where(d => d.IssueDate >= begin);
            }
            else if (!string.IsNullOrEmpty(endDate))
            {
                var end = Convert.ToDateTime(endDate);
                studentBooks = studentBooks.Where(d => d.IssueDate <= end);
            }

            return View(studentBooks.ToList());
        }

        public ActionResult IndexAdmin(string startDate, string endDate)
        {
            var studentBooks = db.StudentBooks.Include(s => s.Book).Include(s => s.Student);
            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                var begin = Convert.ToDateTime(startDate);
                var end = Convert.ToDateTime(endDate);
                studentBooks = studentBooks.Where(d => d.IssueDate >= begin && d.IssueDate <= end);
            }
            else if (!string.IsNullOrEmpty(startDate))
            {
                var begin = Convert.ToDateTime(startDate);
                studentBooks = studentBooks.Where(d => d.IssueDate >= begin);
            }
            else if (!string.IsNullOrEmpty(endDate))
            {
                var end = Convert.ToDateTime(endDate);
                studentBooks = studentBooks.Where(d => d.IssueDate <= end);
            }

            return View(studentBooks.ToList());
        }


        // GET: StudentBooks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentBook studentBook = db.StudentBooks.Find(id);
            if (studentBook == null)
            {
                return HttpNotFound();
            }
            return View(studentBook);
        }

        // GET: StudentBooks/Create
        public ActionResult Create(StudentBook studentBook, int sid, int callNumber)
        {
            studentBook.StudentId = Convert.ToInt32(Session["id"]);
            studentBook.CallNumber = callNumber;
            studentBook.IssueDate = DateTime.Now.Date;
            studentBook.DueDate = DateTime.Now.AddDays(15).Date;
            studentBook.IssueStatus = "Processing";
            studentBook.CheckoutDate = null;
            db.StudentBooks.Add(studentBook);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult BookForFuture([Bind(Include = "CallNumber")] StudentBook studentBook, string datefield)
        {
            studentBook.StudentId = Convert.ToInt32(Session["id"]);
            studentBook.IssueDate = Convert.ToDateTime(datefield).Date;
            studentBook.DueDate = Convert.ToDateTime(datefield).AddDays(15).Date;
            studentBook.IssueStatus = "Processing";
            studentBook.CheckoutDate = null;
            db.StudentBooks.Add(studentBook);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: StudentBooks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(/*[Bind(Include = "StudentId,CallNumber,IssueDate,DueDate,IssueStatus,CheckoutDate")] StudentBook studentBook*/
        //    int sid, int callNumber, string issueDate, string dueDate, string status, string checkOutDate)
        //{
        //    StudentBook studentBook=new StudentBook();
        //    //if (ModelState.IsValid)
        //    //{
        //        studentBook.StudentId = sid;
        //        studentBook.CallNumber = callNumber;
        //        studentBook.IssueDate = Convert.ToDateTime(issueDate);
        //        studentBook.DueDate = Convert.ToDateTime(dueDate);
        //        studentBook.IssueStatus = status;
        //        studentBook.CheckoutDate= Convert.ToDateTime(checkOutDate);
        //        db.StudentBooks.Add(studentBook);
        //        db.SaveChanges();
        //    return RedirectToAction("Index");
        //    //}

        //    ViewBag.CallNumber = new SelectList(db.Books, "CallNumber", "ISBN", studentBook.CallNumber);
        //    ViewBag.StudentId = new SelectList(db.Students, "StudentId", "StudentName", studentBook.StudentId);
        //    return View(studentBook);
        //}

        // GET: StudentBooks/Edit/5
        public ActionResult Edit(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            StudentBook studentBook = db.StudentBooks.SingleOrDefault(s=>s.Id==id);
            if (studentBook == null)
            {
                return HttpNotFound();
            }
            ViewBag.CallNumber = new SelectList(db.Books, "CallNumber", "ISBN", studentBook.CallNumber);
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "StudentName", studentBook.StudentId);
            ViewBag.Status = new SelectList(db.StudentBooks, "IssueStatus", "IssueStatus", studentBook.IssueStatus);
            //return View(studentBook);
            return PartialView("../StudentBooks/_Edit", studentBook);
            //return View(studentBook);
        }

        // POST: StudentBooks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StudentId,CallNumber,IssueDate,DueDate,IssueStatus,CheckoutDate")] StudentBook studentBook)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentBook).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexAdmin");
            }
            ViewBag.CallNumber = new SelectList(db.Books, "CallNumber", "ISBN", studentBook.CallNumber);
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "StudentName", studentBook.StudentId);
            return View(studentBook);
        }

        // GET: StudentBooks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentBook studentBook = db.StudentBooks.Find(id);
            if (studentBook == null)
            {
                return HttpNotFound();
            }
            return View(studentBook);
        }

        // POST: StudentBooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentBook studentBook = db.StudentBooks.Find(id);
            db.StudentBooks.Remove(studentBook);
            db.SaveChanges();
            return RedirectToAction("Index");
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
