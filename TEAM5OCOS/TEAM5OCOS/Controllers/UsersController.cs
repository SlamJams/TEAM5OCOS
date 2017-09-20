/* UC 6: UCAddUser
 * UC 7: UCUpdateUser
 * 
 * Written by: Deandre Hall, Brian Najera
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
    public class UsersController : Controller
    {
        private MSSQLEntities db = new MSSQLEntities();

        // GET: Users
        public async Task<ActionResult> Index()
        {
            string test = (string)Session["userRole"];
            List<string> acceptableRoles = new List<string> {"DBA" };

            if (!acceptableRoles.Contains(test))
                Response.Redirect("~/Login/Login");

            var DBModel = new LogUpdate();
            DBModel.Date = DateTime.Today.ToString("G");
            DBModel.Who = !String.IsNullOrEmpty((string)Session["userName"]) ? (string) Session["UserName"]: (string) Session["userRole"];
            DBModel.Access = "UCViewUsers";
            DBModel.Actual_SQL = "Select * From Users";
            DBModel.TableName = "Users Table";
            db.LogUpdates.Add(DBModel);
            db.SaveChanges();

            return View(await db.Users.ToListAsync());
        }

        
        // GET: Users/Create
        public ActionResult Create()
        {
            string test = (string)Session["userRole"];
            List<string> acceptableRoles = new List<string> { "DBA" };

            if (!acceptableRoles.Contains(test))
                Response.Redirect("~/Login/Login");

            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "UserId,FirstName,LastName,MiddleName,Role,County,District,Email,Phone,Party,AssignedUsername,AssignedPassword")] User user)
        {
            string test = (string)Session["userRole"];
            List<string> acceptableRoles = new List<string> { "DBA" };

            if (!acceptableRoles.Contains(test))
                Response.Redirect("~/Login/Login");
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                await db.SaveChangesAsync();

                var DBModel = new LogUpdate();
                DBModel.Date = DateTime.Today.ToString("G");
                DBModel.Who = !String.IsNullOrEmpty((string)Session["userName"]) ? (string)Session["UserName"] : (string)Session["userRole"];
                DBModel.Access = "UCCreateUser";
                DBModel.Actual_SQL = "Insert into users Values";
                DBModel.TableName = "Users Table";
                db.LogUpdates.Add(DBModel);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            string test = (string)Session["userRole"];
            List<string> acceptableRoles = new List<string> { "DBA" };

            if (!acceptableRoles.Contains(test))
                Response.Redirect("~/Login/Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var DBModel = new LogUpdate();
            DBModel.Date = DateTime.Today.ToString("G");
            DBModel.Who = !String.IsNullOrEmpty((string)Session["userName"]) ? (string)Session["UserName"] : (string)Session["userRole"];
            DBModel.Access = "UCUpdateUsers";
            DBModel.Actual_SQL = "Select * from users where id = search id";
            DBModel.TableName = "Users Table";
            db.LogUpdates.Add(DBModel);
            db.SaveChanges();

            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "UserId,FirstName,LastName,MiddleName,Role,County,District,Email,Phone,Party,AssignedUsername,AssignedPassword")] User user)
        {
            string test = (string)Session["userRole"];
            List<string> acceptableRoles = new List<string> { "DBA" };

            if (!acceptableRoles.Contains(test))
                Response.Redirect("~/Login/Login");

            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                await db.SaveChangesAsync();

                var DBModel = new LogUpdate();
                DBModel.Date = DateTime.Today.ToString("G");
                DBModel.Who = !String.IsNullOrEmpty((string)Session["userName"]) ? (string)Session["UserName"] : (string)Session["userRole"];
                DBModel.Access = "UCUpdateUsers";
                DBModel.Actual_SQL = "Insert into users Values where id = id of search string";
                DBModel.TableName = "Users Table";
                db.LogUpdates.Add(DBModel);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(user);
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
