/* UC 15: UCLogDatabaseAccess
 * 
 * Written by: Roman Soleynik, Simon Tice
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
    public class LogUpdatesController : Controller
    {
        private MSSQLEntities db = new MSSQLEntities();

        // GET: LogUpdates
        public async Task<ActionResult> Index()
        {
            string test = (string)Session["userRole"];
            List<string> acceptableRoles = new List<string> { "DBA" };

            if (!acceptableRoles.Contains(test))
                Response.Redirect("~/Login/Login");
            return View(await db.LogUpdates.ToListAsync());
        }

        // GET: Testimonials
        [HttpPost]
        public ActionResult Index(string searchString)
        {
            string test = (string)Session["userRole"];
            List<string> acceptableRoles = new List<string> { "DBA" };

            if (!acceptableRoles.Contains(test))
                Response.Redirect("~/Login/Login");

            var content = from a in db.LogUpdates
                          select a;

            var DBModel = new LogUpdate();
            DBModel.Date = DateTime.Today.ToString("G");
            DBModel.Who = !String.IsNullOrEmpty((string)Session["userName"]) ? (string)Session["UserName"] : (string)Session["userRole"];
            DBModel.Access = "UCViewLog";
            DBModel.Actual_SQL = "Select * from LogUpdate";
            DBModel.TableName = "Audit Log Table";
            db.LogUpdates.Add(DBModel);
            db.SaveChanges();

            if (!String.IsNullOrEmpty(searchString))
            {
                content = content.Where(s => s.Date.Contains(searchString));
            }

            return View(content);
        }
    }
}






      
