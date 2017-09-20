/* UC 10: UCGetVoterPhoneList
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
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using TEAM5OCOS.Models;

namespace TEAM5OCOS.Controllers
{
    public class VoterPhoneListsController : Controller
    {
        private MSSQLEntities db = new MSSQLEntities();

        public ActionResult Index(string party, string race)
        {
            string test = (string)Session["userRole"];
            List<string> acceptableRoles = new List<string> { "PA", "BCC", "SO", "SE", "DSB", "V", "CC", "TC", "SS", "DBA" };

            if (!acceptableRoles.Contains(test))
                Response.Redirect("~/Login/Login");

            var content = from a in db.VoterPhoneLists
                          select a;

            var DBModel = new LogUpdate();
            DBModel.Date = DateTime.Today.ToString("G");
            DBModel.Who = !String.IsNullOrEmpty((string)Session["userName"]) ? (string)Session["UserName"] : (string)Session["userRole"];
            DBModel.Access = "UCVoterPhoneList";
            DBModel.Actual_SQL = "Select * from view (where criteria are met)";
            DBModel.TableName = "VoterPhone List";
            db.LogUpdates.Add(DBModel);
            db.SaveChanges();

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

            Session["content"] = content.ToList<VoterPhoneList>();


            return View(content);
        }

        

        public ActionResult ExportToCvs()
        {
            var content = (List<VoterPhoneList>)Session["content"];

            GridView gv = new GridView();
            gv.DataSource = content;
            gv.DataBind();

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=VoterPhoneLists.xls");
            Response.Charset = "";

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            gv.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("Index");
        }

       
    }
}
