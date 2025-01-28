using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ResearchCenter.Models;

namespace ResearchCenter.Areas.User.Controllers
{
    public class NationalController : Controller
    {
        // GET: User/National
        [HttpGet]
        public ActionResult NationalList()
        {
            try
            {
                var id = Session["id"] as int?;
                SqlConnectionClass connection = new SqlConnectionClass();
                var dt = connection.Select("select * from NationalPaper where registerId = " + id);

                return View(dt);
            }
            catch (Exception e)
            {
                return View(e);
            }
        }

        [HttpGet]
        public ActionResult SubmitNationalPaper()
        {
            return View(new NationalPaper());
        }

        [HttpPost]
        public ActionResult SubmitNationalPaper(NationalPaper np)
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
                string Name = fileName + Path.GetExtension(np.pdf.FileName);

                if (np.pdf != null && np.pdf.ContentLength > 0)
                {
                    string filePath = Path.Combine(Server.MapPath("~/UploadedFiles"), Name);
                    np.pdf.SaveAs(filePath);
                }

                var id = Session["id"] as int?;
                string condition = "Pending";
                string query = string.Format("INSERT INTO NationalPaper(journal,year,title,faculty,department,designation,name,issue,pdf,comment,status,registerId) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}',{7},'{8}','{9}','{10}',{11})", np.journal, np.year, np.title, np.faculty, np.department, np.designation, np.name, np.issue, Name, np.comment, condition, id);
                SqlConnectionClass sqlCon = new SqlConnectionClass();
                sqlCon.Insert(query);

                return RedirectToAction("NationalList");
            }
            catch (Exception e)
            {
                return View(e);
            }
        }

        [HttpGet]
        public ActionResult EditNational(int id)
        {
            SqlConnectionClass connection = new SqlConnectionClass();
            string query = "select * from NationalPaper where id = " + id;
            var result = connection.Select(query);

            return View(result);
        }

        [HttpPost]
        public ActionResult EditNational(NationalPaper np, int id)
        {
            var date = DateTime.Now;
            var dateTimeString = date.ToString("yyyy-MM-dd HH:mm:ss");
            var dateTime = dateTimeString.GetHashCode();
            if (dateTime < 0)
            {
                dateTime = -dateTime;
            }

            if (np.pdf != null && np.pdf.ContentLength > 0)
            {
                string fileName = dateTime + "file";
                string Name = fileName + Path.GetExtension(np.pdf.FileName);
                string filePath = Path.Combine(Server.MapPath("~/UploadedFiles"), Name);
                np.pdf.SaveAs(filePath);

                string query = "update NationalPaper set pdf = '" + Name + "' where id = " + id;
                SqlConnectionClass con = new SqlConnectionClass();
                con.Update(query);
            }

            if (np.year == DateTime.Today)
            {
                string query = "update NationalPaper set year = '" + np.year + "' where id = " + id;
                SqlConnectionClass con = new SqlConnectionClass();
                con.Update(query);
            }

            string query1 = "update NationalPaper set journal = '" + np.journal + "', title = '" + np.title + "', faculty = '" + np.faculty + "', department = '" + np.department + "', designation = '" + np.designation + "', name = '" + np.name + "', issue = '" + np.issue + "' where id = " + id;
            SqlConnectionClass sqlCon = new SqlConnectionClass();
            sqlCon.Insert(query1);

            return RedirectToAction("NationalList");
        }
    }
}