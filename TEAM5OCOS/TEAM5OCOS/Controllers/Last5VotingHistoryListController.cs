/* UC 12: UCGetVoterHistoryList
 * UC 5: UCExportToExcel
 * 
 * Written by: Deandre Hall
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
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;

namespace TEAM5OCOS.Controllers
{
    public class Last5VotingHistoryListController : Controller
    {
        private MSSQLEntities db = new MSSQLEntities();

        // GET: Last5VotingHistoryList
        public ActionResult Index(string party, string race)
        {
            string test = (string)Session["userRole"];
            List<string> acceptableRoles = new List<string> { "PA", "BCC", "SO", "SE", "DSB", "V", "CC", "TC", "SS", "DBA" };

            if (!acceptableRoles.Contains(test))
                Response.Redirect("~/Login/Login");

            var content = from a in db.Last5VotingHistoryList
                          select a;

            if (!String.IsNullOrEmpty(party) && !String.IsNullOrEmpty(race))
            {
                content = content.Where(
                    s =>
                    (s.party.Contains(party)) &&
                    (s.description.Contains(race))
                );
            }
            else if (!String.IsNullOrEmpty(party) && String.IsNullOrEmpty(race))
            {
                content = content.Where(
                    s =>
                    (s.party.Contains(party))
                );
            }
            else if (String.IsNullOrEmpty(party) && !String.IsNullOrEmpty(race))
            {
                content = content.Where(
                    s =>
                    (s.description.Contains(race))
                );
            }

            Session["content"] = content.ToList<Last5VotingHistoryList>();

            var DBModel = new LogUpdate();
            DBModel.Date = DateTime.Today.ToString("G");
            DBModel.Who = !String.IsNullOrEmpty((string)Session["userName"]) ? (string)Session["UserName"] : (string)Session["userRole"];
            DBModel.Access = "UCViewLast5VotingHistory";
            DBModel.Actual_SQL = "Select * from view (where criteria are met)";
            DBModel.TableName = "Last5VotingHistory List";
            db.LogUpdates.Add(DBModel);
            db.SaveChanges();

            return View(content);
        }

        public ActionResult ExportToCvs()
        {
            string test = (string)Session["userRole"];
            List<string> acceptableRoles = new List<string> { "PA", "BCC", "SO", "SE", "DSB", "V", "CC", "TC", "SS", "DBA" };

            if (!acceptableRoles.Contains(test))
                Response.Redirect("~/Login/Login");
            var content = (List<Last5VotingHistoryList>)Session["content"];

            GridView gv = new GridView();
            gv.DataSource = content;
            gv.DataBind();

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=Last5VotingHistoryList.xls");
            Response.Charset = "";

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            gv.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

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
