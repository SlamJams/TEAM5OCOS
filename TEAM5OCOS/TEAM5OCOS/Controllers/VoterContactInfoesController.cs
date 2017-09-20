/* UC 13: UCUpdateVoterPhoneAndEmail
 * UC 5: UCExportToExcel
 * 
 * Written by: Khanh-Linh Tran
 * Date: 04/23/17
 * 
 * Approved By: 
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
    public class VoterContactInfoesController : Controller
    {
        private MSSQLEntities db = new MSSQLEntities();

        public ActionResult Index(string search)
        {

            Session["voterIDSearch"] = search;
            string test = null;
            List<string> acceptableRoles = new List<string> { "V", "DBA" };
            int? voterid = null;

            if (Session["userRole"] != null)
            {
                test = (string)Session["userRole"];
            }
            if (Session["voterID"] != null)
            {
               voterid = (int)Session["voterID"];
            }

            if (!acceptableRoles.Contains(test) && voterid == null)
                Response.Redirect("~/Login/VoterLogin");

            var content = from a in db.VoterContactInfoes
                          select a;

            var DBModel = new LogUpdate();
            DBModel.Date = DateTime.Today.ToString("G");
            DBModel.Who = !String.IsNullOrEmpty((string)Session["userName"]) ? (string)Session["UserName"] : (string)Session["userRole"];
            DBModel.Access = "UCVoterContactInfo";
            DBModel.Actual_SQL = "Select * from table";
            DBModel.TableName = "VoterContactInfo List";
            db.LogUpdates.Add(DBModel);
            db.SaveChanges();

            if (!String.IsNullOrEmpty(search))
            {
                int searchInt = Int32.Parse(search);
                content = content.Where(s => s.VoterId == searchInt);
            }
            else
            {
                content = null;
            }


            return View(content);
        }


        // GET: VoterContactInfoes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            string test = null;
            List<string> acceptableRoles = new List<string> { "V", "DBA" };
            int? voterid = null;

            if (Session["userRole"] != null)
            {
                test = (string)Session["userRole"];
            }
            if (Session["voterID"] != null)
            {
                voterid = (int)Session["voterID"];

            }

            if (!acceptableRoles.Contains(test) && voterid == null)
                Response.Redirect("~/Login/VoterLogin");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VoterContactInfo voterContactInfo = await db.VoterContactInfoes.FindAsync(id);

            var DBModel = new LogUpdate();
            DBModel.Date = DateTime.Today.ToString("G");
            DBModel.Who = !String.IsNullOrEmpty((string)Session["userName"]) ? (string)Session["UserName"] : (string)Session["userRole"];
            DBModel.Access = "UCUpdateVoter";
            DBModel.Actual_SQL = "Select * from table where voterId is searchID";
            DBModel.TableName = "VoterContactInfo List";
            db.LogUpdates.Add(DBModel);
            db.SaveChanges();

            if (voterContactInfo == null)
            {
                return HttpNotFound();
            }

            return View(voterContactInfo);
        }

        // POST: VoterContactInfoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include ="contactId,Phone,VoteEmail,PublicEmail")]VoterContactInfo voterContactInfo)
        {
            if (ModelState.IsValid)
            {
                VoterContactInfo v = new VoterContactInfo { contactId = voterContactInfo.contactId };
                db.VoterContactInfoes.Attach(v);

                v.Phone = voterContactInfo.Phone;
                v.VoteEmail = voterContactInfo.VoteEmail;
                v.PublicEmail = voterContactInfo.PublicEmail;

                await db.SaveChangesAsync();

                var DBModel = new LogUpdate();
                DBModel.Date = DateTime.Today.ToString("G");
                DBModel.Who = !String.IsNullOrEmpty((string)Session["userName"]) ? (string)Session["UserName"] : (string)Session["userRole"];
                DBModel.Access = "UCUpdateVoter";
                DBModel.Actual_SQL = "Insert into table values where voterID = searchID";
                DBModel.TableName = "VoterContactInfo List";
                db.LogUpdates.Add(DBModel);
                db.SaveChanges();

                return RedirectToAction("Index", new { search = Session["voterIDSearch"]});
            }
            return View(voterContactInfo);
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
