using ResearchCenter.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ResearchCenter.Controllers
{
    public class AcceptableJournalController : Controller
    {
        // GET: AcceptableJournal
        public ActionResult ShowAcceptableJournal()
        {
            SqlConnectionClass connection = new SqlConnectionClass();
            string query = "select * from AcceptableJournal order by id desc";
            var result = connection.Select(query);

            return View(result);
        }

        [HttpGet]
        public ActionResult CreateAcceptableJournal()
        {
            return View(new AcceptableJournal());
        }
        [HttpPost]
        public ActionResult CreateAcceptableJournal(AcceptableJournal acceptableJournal)
        {

            string Name = Path.GetFileName(acceptableJournal.logo.FileName);

            if (acceptableJournal.logo != null && acceptableJournal.logo.ContentLength > 0)
            {
                string fileName = Path.GetFileName(acceptableJournal.logo.FileName);
                string filePath = Path.Combine(Server.MapPath("~/UploadedFiles"), fileName);
                acceptableJournal.logo.SaveAs(filePath);
            }

            SqlConnectionClass connection = new SqlConnectionClass();
            string query = string.Format("INSERT INTO AcceptableJournal(logo) VALUES('{0}')", Name);
            connection.Insert(query);

            return RedirectToAction("ShowAcceptableJournal");
        }
        [HttpGet]
        public ActionResult EditAcceptableJournal(int id)
        {
            SqlConnectionClass connection = new SqlConnectionClass();
            var result = connection.Select("select * from AcceptableJournal where id = " + id);
            return View(result);
        }
        [HttpPost]
        public ActionResult EditAcceptableJournal(AcceptableJournal acceptableJournal, int id)
        {
            SqlConnectionClass connection = new SqlConnectionClass();

            if (acceptableJournal.logo != null && acceptableJournal.logo.ContentLength > 0)
            {
                string fileName = Path.GetFileName(acceptableJournal.logo.FileName);
                string filePath = Path.Combine(Server.MapPath("~/UploadedFiles"), fileName);
                acceptableJournal.logo.SaveAs(filePath);

                string query = "update AcceptableJournal set logo = '" + fileName + "' where id = " + id;
                connection.Insert(query);
            }

            return RedirectToAction("ShowAcceptableJournal");
        }

        public ActionResult DeleteAcceptableJournal(int id)
        {
            SqlConnectionClass connection = new SqlConnectionClass();
            string query = "delete from AcceptableJournal where id = " + id;
            connection.Delete(query);
            return RedirectToAction("ShowAcceptableJournal");
        }
    }
}