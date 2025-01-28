using ResearchCenter.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ResearchCenter.Controllers
{
    public class NationalPaperController : Controller
    {
        // GET: NationalPaper
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                SqlConnectionClass connection = new SqlConnectionClass();
                var dt = connection.Select("select * from NationalPaper order by id desc");

                return View(dt);
            }
            catch (Exception e)
            {
                return View(e);
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new NationalPaper());
        }

        [HttpPost]
        public ActionResult Create(NationalPaper obj)
        {
            try
            {
                var date = DateTime.Now;
                var dateTimeString = date.ToString("yyyy-MM-dd HH:mm:ss");
                var dateTime = dateTimeString.GetHashCode();
                if (dateTime < 0)
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

                string condition = "Pending";
                string query = string.Format("INSERT INTO NationalPaper(journal,year,title,faculty,department,designation,name,issue,pdf,comment,status) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}',{7},'{8}','{9}','{10}')", obj.journal, obj.year, obj.title, obj.faculty, obj.department, obj.designation, obj.name, obj.issue, Name, obj.comment, condition);
                SqlConnectionClass sqlCon = new SqlConnectionClass();
                sqlCon.Insert(query);

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View(e);
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            SqlConnectionClass connection = new SqlConnectionClass();
            string query = "select * from NationalPaper where id = " + id;
            var result = connection.Select(query);

            return View(result);
        }

        [HttpPost]
        public ActionResult Edit(NationalPaper ip, int id)
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

                string query = "update NationalPaper set pdf = '" + Name + "' where id = " + id;
                SqlConnectionClass obj = new SqlConnectionClass();
                obj.Update(query);
            }

            if (ip.year != null)
            {
                string query = "update NationalPaper set year = '" + ip.year + "' where id = " + id;
                SqlConnectionClass obj = new SqlConnectionClass();
                obj.Update(query);
            }

            string query1 = "update NationalPaper set journal = '" + ip.journal + "', title = '" + ip.title + "', faculty = '" + ip.faculty + "', department = '" + ip.department + "', designation = '" + ip.designation + "', name = '" + ip.name + "', issue = '" + ip.issue + "', comment = '" + ip.comment + "' where id = " + id;
            SqlConnectionClass con = new SqlConnectionClass();
            con.Update(query1);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            try
            {
                SqlConnectionClass connection = new SqlConnectionClass();
                var data = connection.Select("select * from NationalPaper where id = " + id);

                if (data != null)
                {
                    string query = "delete from NationalPaper where id = " + id;
                    connection.Delete(query);
                }
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "The record is not found please try again";
                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult ChangeStatus(int id, string status)
        {
            SqlConnectionClass connection = new SqlConnectionClass();

            if (status == "1")
            {
                connection.Update("update NationalPaper set status = 'Confirmed' where id = " + id);
                return RedirectToAction("Index");
            }
            else
            {
                connection.Update("update NationalPaper set status = 'Rejected' where id = " + id);
                return RedirectToAction("Index");
            }
        }

        public JsonResult Search(string query)
        {
            SqlConnectionClass connection = new SqlConnectionClass();
            string q = "SELECT * FROM NationalPaper WHERE name = '" + query + "' OR designation = '" + query + "' OR faculty = '" + query + "'";
            DataTable result = new DataTable();
            result = connection.Select(q);

            List<NationalPaper> listResult = new List<NationalPaper>();
            for (int i = 0; i < result.Rows.Count; i++)
            {
                listResult.Add(new NationalPaper() { id = Convert.ToInt32(result.Rows[i][0]), journal = Convert.ToString(result.Rows[i][1]), year = (DateTime)(result.Rows[i][2]), title = Convert.ToString(result.Rows[i][3]), faculty = Convert.ToString(result.Rows[i][4]), department = Convert.ToString(result.Rows[i][5]), designation = Convert.ToString(result.Rows[i][6]), name = Convert.ToString(result.Rows[i][7]), issue = Convert.ToInt32(result.Rows[i][8]), pdfFile = result.Rows[i][9].ToString(), comment = Convert.ToString(result.Rows[i][10]), status = Convert.ToString(result.Rows[i][11]) });
            }
            //listResult = WebHomeController.ConvertToList<InternationalPaper>(result);

            return Json(listResult, JsonRequestBehavior.AllowGet);
        }
    }
}