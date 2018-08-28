using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Lib.Models;
using System.IO;
using PagedList;

namespace Lib.Controllers
{
    public class BooksController : Controller
    {
        private ModelContext db = new ModelContext();
        // GET: Books
        public ActionResult Index( string txtTitle, string id, int? page)
        {
            List<Category> catelist = db.Categories.ToList();
            var books = db.Books.Include(b => b.Category);
            if (!String.IsNullOrEmpty(id) && !string.IsNullOrEmpty(txtTitle))
            {
                books = books.Where(i => i.CategoryId.ToString() == id && i.Title.Contains(txtTitle));
            }
            else if (!string.IsNullOrEmpty(txtTitle))
            {
                books = books.Where(i => i.Title.Contains(txtTitle));
            }
            else if (!string.IsNullOrEmpty(id))
            {
                books = books.Where(i => i.CategoryId.ToString() == id);
            }
            ViewBag.CategoryId = catelist;
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(books.ToList().ToPagedList(pageNumber, pageSize));
        }

        public ActionResult IndexGuest(string txtTitle, string id, int? page)
        {
            //List<Category> catelist = db.Categories.ToList();
            //var books = db.Books.Include(b => b.Category);
            //if (!String.IsNullOrEmpty(id))
            //{
            //    books = books.Where(i => i.CategoryId.ToString() == id);
            //}
            //ViewBag.CategoryId = catelist;
            //return View(books.ToList());
            List<Category> catelist = db.Categories.ToList();
            var books = db.Books.Include(b => b.Category);
            if (!String.IsNullOrEmpty(id) && !string.IsNullOrEmpty(txtTitle))
            {
                books = books.Where(i => i.CategoryId.ToString() == id && i.Title.Contains(txtTitle));
            }
            else if (!string.IsNullOrEmpty(txtTitle))
            {
                books = books.Where(i => i.Title.Contains(txtTitle));
            }
            else if (!string.IsNullOrEmpty(id))
            {
                books = books.Where(i => i.CategoryId.ToString() == id);
            }
            ViewBag.CategoryId = catelist;
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(books.ToList().ToPagedList(pageNumber, pageSize));
        }
        public ActionResult IndexAdmin()
        {
            var books = db.Books.Include(b => b.Category);
            return View(books.ToList());
        }
        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            Book book = new Book();
            book = db.Books.Find(id);
            return PartialView("../Books/_BookDetails", book);
        }

        public ActionResult DetailsAdmin(int? id)
        {
            Book book = new Book();
            book = db.Books.Find(id);
            return PartialView("../Books/_BookDetailsAdmin", book);
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName");
            return PartialView("../Books/_BookCreate");
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CallNumber,ISBN,Title,Author,Image,CategoryId")] Book book, HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    string imgPath = Path.GetFileName(file.FileName);
                    string url = Path.Combine(Server.MapPath("~/Img/Book/"), imgPath);
                    file.SaveAs(url);
                    book.Image = "/Img/Book/" + imgPath;
                }
                if (ModelState.IsValid)
                {
                    db.Books.Add(book);
                    db.SaveChanges();
                    return RedirectToAction("IndexAdmin");
                }
            }
            catch (Exception e)
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Cannot create this record');</script>");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", book.CategoryId);
            return PartialView("../Books/_BookCreate", book);
        }

        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", book.CategoryId);
            //return View(book);
            return PartialView("../Books/_BookEdit", book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CallNumber,ISBN,Title,Author,CategoryId")] Book book/*, HttpPostedFileBase file*/)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Entry(book).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("IndexAdmin");
            //}
            //try
            //{
                //if (file.ContentLength > 0)
                //{
                //    string imgPath = Path.GetFileName(file.FileName);
                //    string url = Path.Combine(Server.MapPath("~/Img/Book/"), imgPath);
                //    file.SaveAs(url);
                //    book.Image = "/Img/Book/" + imgPath;
                //}
                if (ModelState.IsValid)
                {
                    db.Entry(book).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("IndexAdmin");
                }
            //}
            //catch (Exception)
            //{

            //    throw;
            //}
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", book.CategoryId);
            //return View(book);
            return PartialView("../Books/_BookEdit", book);
        }

        // GET: Books/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return PartialView("../Books/_BookDelete", book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //try
            //{
                Book book = db.Books.Find(id);
                db.Books.Remove(book);
                db.SaveChanges();
                return RedirectToAction("IndexAdmin");
            //}
            //catch (Exception)
            //{

            //    return Content("<script language='javascript' type='text/javascript'>alert('Cannot delete this record');</script>");
            //}
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
