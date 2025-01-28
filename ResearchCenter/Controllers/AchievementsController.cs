using ResearchCenter.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ResearchCenter.Controllers
{
    public class AchievementsController : Controller
    {
        // GET: Achievements
        public ActionResult ShowAchievements()
        {
            SqlConnectionClass connection = new SqlConnectionClass();
            string query = "select * from Achievement order by id desc";
            var result = connection.Select(query);

            return View(result);
        }

        [HttpGet]
        public ActionResult CreateAchievements()
        {
            return View(new Achievement());
        }
        [HttpPost]
        public ActionResult CreateAchievements(Achievement achievement)
        {

            string Name = Path.GetFileName(achievement.image.FileName);

            if (achievement.image != null && achievement.image.ContentLength > 0)
            {
                string fileName = Path.GetFileName(achievement.image.FileName);
                string filePath = Path.Combine(Server.MapPath("~/UploadedFiles"), fileName);
                achievement.image.SaveAs(filePath);
            }

            SqlConnectionClass connection = new SqlConnectionClass();
            string query = string.Format("INSERT INTO Achievement(title,description,image) VALUES('{0}','{1}','{2}')", achievement.title, achievement.description, Name);
            connection.Insert(query);

            return RedirectToAction("ShowAchievements");
        }
        [HttpGet]
        public ActionResult EditAchievements(int id)
        {
            SqlConnectionClass connection = new SqlConnectionClass();
            var result = connection.Select("select * from Achievement where id = " + id);
            return View(result);
        }
        [HttpPost]
        public ActionResult EditAchievements(Achievement achievement, int id)
        {
            SqlConnectionClass connection = new SqlConnectionClass();

            if (achievement.image != null && achievement.image.ContentLength > 0)
            {
                string fileName = Path.GetFileName(achievement.image.FileName);
                string filePath = Path.Combine(Server.MapPath("~/UploadedFiles"), fileName);
                achievement.image.SaveAs(filePath);

                string query = "update Achievement set image = '" + fileName + "' where id = " + id;
                connection.Insert(query);
            }

            string query1 = "update Achievement set title = '" + achievement.title + "', description = '" + achievement.description + "' where id = " + id;
            connection.Insert(query1);

            return RedirectToAction("ShowAchievements");
        }

        public ActionResult DeleteAchievements(int id)
        {
            SqlConnectionClass connection = new SqlConnectionClass();
            string query = "delete from Achievement where id = " + id;
            connection.Delete(query);
            return RedirectToAction("ShowAchievements");
        }
    }
}