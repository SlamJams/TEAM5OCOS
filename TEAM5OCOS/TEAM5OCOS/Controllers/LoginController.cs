/* UC 14: UCLogin
 * 
 * Written by: Brian Najera, Khanh-Linh Tran
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
using System.Web.Security;

namespace TEAM5OCOS.Controllers
{
    public class LoginController : Controller
    {
        private MSSQLEntities db = new MSSQLEntities();

        // GET: Login
        public ActionResult Index()
        {
            string test = (string)Session["userRole"];
            List<string> acceptableRoles = new List<string> { "CS", "PA", "BCC", "SO", "SE", "DSB", "V", "CC", "TC", "SS", "DBA" };

                if (!acceptableRoles.Contains(test))
                    Response.Redirect("~/Login/Login");
            return View();
        }

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        // GET: Login
        [HttpPost]
        public ActionResult Login(User model)
        {


            var dataItem = db.Users.FirstOrDefault(x => x.AssignedUsername == model.AssignedUsername && x.AssignedPassword == model.AssignedPassword);
            if (dataItem != null)
            {

                var DBModel = new LogUpdate();
                DBModel.Date = DateTime.Today.ToString("G");
                DBModel.Who = !String.IsNullOrEmpty((string)Session["userName"]) ? (string)Session["UserName"] : (string)Session["userRole"];
                DBModel.Access = "UCLogin";
                DBModel.Actual_SQL = "Select * from users table where username and password match";
                DBModel.TableName = "Users Table";
                db.LogUpdates.Add(DBModel);
                db.SaveChanges();

                Session["userFirstName"] = dataItem.FirstName;
                Session["userMiddleName"] = dataItem.MiddleName;
                Session["userLastName"] = dataItem.LastName;
                Session["userRole"] = dataItem.Role;
                switch (dataItem.Role)
                {
                    case "CC":
                        Session["userTitle"] = "Clerk Of Court";
                        break;
                    case "PA":
                        Session["userTitle"] = "Property Appraiser";
                        break;
                    case "SO":
                        Session["userTitle"] = "Sherriff's Office";
                        break;
                    case "SE":
                        Session["userTitle"] = "Supervisor of Elections";
                        break;
                    case "TC":
                        Session["userTitle"] = "Tax Collector";
                        break;
                    case "SS":
                        Session["userTitle"] = "School Superintendent";
                        break;
                    case "BCC":
                        Session["userTitle"] = "Board of County Commissioners";
                        break;
                    case "DSB":
                        Session["userTitle"] = "District School Board";
                        break;
                    case "CS":
                        Session["userTitle"] = "Customer Service";
                        break;
                    case "V":
                        Session["userTitle"] = "Volunteer";
                        break;
                    case "DBA":
                        Session["userTitle"] = "Database Administrator";
                        break;
                }
             return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Invalid user or password");
                return View();
            }

        }

        // GET: Login
        public ActionResult VoterLogin()
        {
            return View();
        }

        // GET: VoterLogin
        [HttpPost]
        public ActionResult VoterLogin(VoterInfo model)
        {
            var dataItem = db.VoterInfoes.FirstOrDefault(x => x.VoterId == model.VoterId && x.VFirstName == model.VFirstName && x.VLastName == model.VLastName);
            if (dataItem != null)
            {
                var DBModel = new LogUpdate();
                DBModel.Date = DateTime.Today.ToString("G");
                DBModel.Who = !String.IsNullOrEmpty((string)Session["userName"]) ? (string)Session["UserName"] : (string)Session["userRole"];
                DBModel.Access = "UCVoterLogin";
                DBModel.Actual_SQL = "Select * from VoterInfo where VoterID and Last Name and First name match search";
                DBModel.TableName = "VoterInfo";
                db.LogUpdates.Add(DBModel);
                db.SaveChanges();

                Session["voterName"] = dataItem.VFirstName;
                Session["voterID"] = dataItem.VoterId;

                return RedirectToAction("Index", "VoterContactInfoes", new {search = (int)Session["voterID"] });
            }
            else
            {
                ModelState.AddModelError("", "Invalid login information");
                return View();
            }

        }

        public ActionResult SignOut()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

    }
}
