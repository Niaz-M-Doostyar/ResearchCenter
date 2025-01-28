using ResearchCenter.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ResearchCenter.Controllers
{
    public class PredatoryJournalController : Controller
    {
        // GET: PredatoryJournal
        public ActionResult ShowPredatoryJournal()
        {
            SqlConnectionClass connection = new SqlConnectionClass();
            string query = "select * from PredatoryJournal order by id desc";
            var result = connection.Select(query);

            return View(result);
        }

        [HttpGet]
        public ActionResult CreatePredatoryJournal()
        {
            return View(new PredatoryJournal());
        }
        [HttpPost]
        public ActionResult CreatePredatoryJournal(PredatoryJournal predatoryJournal)
        {

            SqlConnectionClass connection = new SqlConnectionClass();
            string query = string.Format("INSERT INTO PredatoryJournal(link) VALUES('{0}')", predatoryJournal.link);
            connection.Insert(query);

            return RedirectToAction("ShowPredatoryJournal");
        }
        [HttpGet]
        public ActionResult EditPredatoryJournal(int id)
        {
            SqlConnectionClass connection = new SqlConnectionClass();
            var result = connection.Select("select * from PredatoryJournal where id = " + id);
            return View(result);
        }
        [HttpPost]
        public ActionResult EditPredatoryJournal(PredatoryJournal predatoryJournal, int id)
        {
            SqlConnectionClass connection = new SqlConnectionClass();

            string query1 = "update PredatoryJournal set link = '" + predatoryJournal.link + "' where id = " + id;
            connection.Insert(query1);

            return RedirectToAction("ShowAchievements");
        }

        public ActionResult DeletePredatoryJournal(int id)
        {
            SqlConnectionClass connection = new SqlConnectionClass();
            string query = "delete from PredatoryJournal where id = " + id;
            connection.Delete(query);
            return RedirectToAction("ShowPredatoryJournal");
        }
    }
}