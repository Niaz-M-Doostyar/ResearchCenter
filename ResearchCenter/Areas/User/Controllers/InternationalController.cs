using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ResearchCenter.Models;

namespace ResearchCenter.Areas.User.Controllers
{
    public class InternationalController : Controller
    {
        // GET: User/International
        public ActionResult InternationalList()
        {
            try
            {
                var id = Session["id"] as int?;
                SqlConnectionClass connection = new SqlConnectionClass();
                var dt = connection.Select("select * from InternationalPaper where registerId = " + id);

                return View(dt);
            }
            catch (Exception e)
            {
                return View(e);
            }
        }

        [HttpGet]
        public ActionResult SubmitInternationalPaper()
        {
            return View(new InternationalPaper());
        }

        [HttpPost]
        public ActionResult SubmitInternationalPaper(InternationalPaper obj)
        {
            try
            {
                var date = DateTime.Now;
                var dateTimeString = date.ToString("yyyy-MM-dd HH:mm:ss");
                var dateTime = dateTimeString.GetHashCode();
                if(dateTime < 0)
                {
                    dateTime = -dateTime;
                }

                string fileName = dateTime + "file";
                string Name = fileName + Path.GetExtension(obj.pdf.FileName);

                if (obj.pdf != null && obj.pdf.ContentLength > 0)
                {
                    string filePath = Path.Combine(Server.MapPath("~/UploadedFiles"), Name);
                    obj.pdf.SaveAs(filePath);
                }

                var id = Session["id"] as int?;
                string condition = "Pending";
                string speciality = "0";
                string query = string.Format("INSERT INTO InternationalPaper(familyName,name,authors,department,faculty,designation,speciality,title,artical,journal,journalName,language,year,issue,page,doi,pdf,comment,status,registerId) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}',{13},{14},'{15}','{16}','{17}','{18}',{19})", obj.familyName, obj.name, obj.authors, obj.department, obj.faculty, obj.designation, speciality, obj.title, obj.artical, obj.journal, obj.journalName, obj.language, obj.year, obj.issue, obj.page, obj.doi, Name, obj.comment, condition, id);
                SqlConnectionClass sqlCon = new SqlConnectionClass();
                sqlCon.Insert(query);

                return RedirectToAction("InternationalList");
            }
            catch (Exception e)
            {
                return View(e);
            }
        }

        [HttpGet]
        public ActionResult EditInternational(int id)
        {
            SqlConnectionClass connection = new SqlConnectionClass();
            string query = "select * from InternationalPaper where id = " + id;
            var result = connection.Select(query);

            return View(result);
        }

        [HttpPost]
        public ActionResult EditInternational(InternationalPaper ip, int id)
        {
            var date = DateTime.Now;
            var dateTimeString = date.ToString("yyyy-MM-dd HH:mm:ss");
            var dateTime = dateTimeString.GetHashCode();
            if (dateTime < 0)
            {
                dateTime = -dateTime;
            }

            if (ip.pdf != null && ip.pdf.ContentLength > 0)
            {
                string fileName = dateTime + "file";
                string Name = fileName + Path.GetExtension(ip.pdf.FileName);
                string filePath = Path.Combine(Server.MapPath("~/UploadedFiles"), Name);
                ip.pdf.SaveAs(filePath);

                string query = "update InternationalPaper set pdf = '" + Name + "' where id = " + id;
                SqlConnectionClass con = new SqlConnectionClass();
                con.Update(query);
            }

            if (ip.year == DateTime.Today)
            {
                string query = "update InternationalPaper set year = '" + ip.year + "' where id = " + id;
                SqlConnectionClass con = new SqlConnectionClass();
                con.Update(query);
            }

            string speciality = "0";
            string query1 = "update InternationalPaper set familyName = '" + ip.familyName + "', name = '" + ip.name + "', authors = '" + ip.authors + "', department = '" + ip.department + "', faculty = '" + ip.faculty + "', designation = '" + ip.designation + "', speciality = '" + speciality + "', title = '" + ip.title + "', artical = '" + ip.artical + "', journal = '" + ip.journal + "', journalName = '" + ip.journalName + "', language = '" + ip.language + "', issue = '" + ip.issue + "', page = '" + ip.page + "', doi = '" + ip.doi + "' where id = " + id;
            SqlConnectionClass sqlCon = new SqlConnectionClass();
            sqlCon.Insert(query1);

            return RedirectToAction("InternationalList");
        }
    }
}