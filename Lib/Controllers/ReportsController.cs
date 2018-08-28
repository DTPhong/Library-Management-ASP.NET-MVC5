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
    public class ReportsController : Controller
    {
        private ModelContext db = new ModelContext();

        // GET: Reports
        public ActionResult Index(string smonth, string syear)
        {
            int Month = Convert.ToInt32(smonth);
            long Year = Convert.ToInt64(syear);
            var category = db.Categories;
            var stBook = db.StudentBooks.Where(sb => sb.IssueDate.Value.Month == Month && sb.IssueDate.Value.Year == Year);
            List<Report> report = new List<Report>();

            foreach (var categoryItem in category)
            {
                var Count = 0;
                foreach (var stuBookItem in stBook.Where(cate => cate.Book.CategoryId == categoryItem.CategoryId))
                {
                    Count++;
                }
                report.Add(new Report()
                {
                    CategoryId = categoryItem.CategoryId,
                    CategoryName = categoryItem.CategoryName,
                    NumberOfBorrowed = Count,
                });
            }
           
            return View(report);
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