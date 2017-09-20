/* UC 2: UCEnterTestimonial
 * UC 3: UCSearchTestimonial
 * 
 * Written by: Brian Najera, Deandre Hall, Simon Tice
 * Date: 04/26/17
 * 
 * Approved By: Khanh-Linh Tran
 * Date: 04/26/17
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TEAM5OCOS.Models;

namespace TEAM5OCOS.Controllers
{
    public class TestimonialsController : Controller
    {
        private MSSQLEntities db = new MSSQLEntities();

        // GET: Testimonials
        public ActionResult Index(string searchString)
        {
            var content = from a in db.Testimonials
                select a;

            var DBModel = new LogUpdate();
            DBModel.Date = DateTime.Today.ToString("G");
            DBModel.Who = !String.IsNullOrEmpty((string)Session["userName"]) ? (string)Session["UserName"] : (string)Session["userRole"];
            DBModel.Access = "UCSearchTestimonials";
            DBModel.Actual_SQL = "Select * from testimonials table";
            DBModel.TableName = "Testimonials";
            db.LogUpdates.Add(DBModel);
            db.SaveChanges();

            if (!String.IsNullOrEmpty(searchString))
            {
                content = content.Where(s => s.T_Content.Contains(searchString));
            }

            return View(content);
        }

           
        // GET: Testimonials/Create
        public ActionResult Create()

        {
            string test = (string)Session["userRole"];
            List<string> acceptableRoles = new List<string> {"PA", "BCC", "SO", "SE", "DSB", "V", "CC", "TC", "SS", "DBA" };

            if (!acceptableRoles.Contains(test))
                Response.Redirect("~/Login/Login");
            return View();
        }

        // POST: Testimonials/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "T_ID,T_Date,T_Content")] Testimonial testimonial)
        {
            if (ModelState.IsValid)
            {
                db.Testimonials.Add(testimonial);
                await db.SaveChangesAsync();

                var DBModel = new LogUpdate();
                DBModel.Date = DateTime.Today.ToString("G");
                DBModel.Who = !String.IsNullOrEmpty((string)Session["userName"]) ? (string)Session["UserName"] : (string)Session["userRole"];
                DBModel.Access = "UCAddTestimonial";
                DBModel.Actual_SQL = "Insert into testimonials Values";
                DBModel.TableName = "Testimonials Table";
                db.LogUpdates.Add(DBModel);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(testimonial);
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
