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
    public class InternationalPaperController : Controller
    {
        // GET: InternationalPaper

        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                SqlConnectionClass connection = new SqlConnectionClass();
                var dt = connection.Select("select * from InternationalPaper order by id desc");

                return View(dt);
            }
            catch(Exception e)
            {
                return View(e);
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new InternationalPaper());
        }

        [HttpPost]
        public ActionResult Create(InternationalPaper obj)
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

                string condition = "Pending";
                string speciality = "0";
                string query = string.Format("INSERT INTO InternationalPaper(familyName,name,authors,department,faculty,designation,speciality,title,artical,journal,journalName,language,year,issue,page,doi,pdf,comment,status) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}',{13},{14},'{15}','{16}','{17}','{18}')", obj.familyName, obj.name, obj.authors, obj.department, obj.faculty, obj.designation, speciality, obj.title, obj.artical, obj.journal, obj.journalName, obj.language, obj.year, obj.issue, obj.page, obj.doi, Name, obj.comment, condition);
                SqlConnectionClass sqlCon = new SqlConnectionClass();
                sqlCon.Insert(query);

                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                return View(e);
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            SqlConnectionClass connection = new SqlConnectionClass();
            string query = "select * from InternationalPaper where id = " + id;
            var result = connection.Select(query);

            return View(result);
        }

        [HttpPost]
        public ActionResult Edit(InternationalPaper ip, int id)
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
                SqlConnectionClass obj = new SqlConnectionClass();
                obj.Insert(query);
            }

            if (ip.year != null)
            {
                string query = "update InternationalPaper set year = '" + ip.year + "' where id = " + id;
                SqlConnectionClass obj = new SqlConnectionClass();
                obj.Insert(query);
            }
            string speciality = "0";
            string query1 = "update InternationalPaper set familyName = '" + ip.familyName + "', name = '" + ip.name + "', authors = '" + ip.authors + "', department = '" + ip.department + "', faculty = '" + ip.faculty + "', designation = '" + ip.designation + "', speciality = '" + speciality + "', title = '" + ip.title + "', artical = '" + ip.artical + "', journal = '" + ip.journal + "', journalName = '" + ip.journalName + "', language = '" + ip.language + "', issue = '" + ip.issue + "', page = '" + ip.page + "', doi = '" + ip.doi + "', comment = '" + ip.comment + "' where id = " + id;
            SqlConnectionClass con = new SqlConnectionClass();
            con.Insert(query1);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            try
            {
                SqlConnectionClass connection = new SqlConnectionClass();
                var data = connection.Select("select * from InternationalPaper where id = " + id);

                if (data != null)
                {
                    string query = "delete from InternationalPaper where id = " + id;
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
            
            if(status == "1")
            {
                connection.Update("update InternationalPaper set status = 'Confirmed' where id = " + id);
                return RedirectToAction("Index");
            }
            else
            {
                connection.Update("update InternationalPaper set status = 'Rejected' where id = " + id);
                return RedirectToAction("Index");
            }
        }

        public JsonResult Search(string query)
        {
            SqlConnectionClass connection = new SqlConnectionClass();
            string q = "SELECT * FROM InternationalPaper WHERE name = '" + query + "' OR designation = '" + query + "' OR faculty = '" + query + "'";
            DataTable result = new DataTable();
            result = connection.Select(q);

            List<InternationalPaper> listResult = new List<InternationalPaper>();
            for (int i = 0; i < result.Rows.Count; i++)
            {
                listResult.Add(new InternationalPaper() { id = Convert.ToInt32(result.Rows[i][0]), familyName = Convert.ToString(result.Rows[i][1]), name = Convert.ToString(result.Rows[i][2]), authors = Convert.ToString(result.Rows[i][3]), department = Convert.ToString(result.Rows[i][4]), faculty = Convert.ToString(result.Rows[i][5]), designation = Convert.ToString(result.Rows[i][6]), speciality = Convert.ToString(result.Rows[i][7]), title = Convert.ToString(result.Rows[i][8]), artical = Convert.ToString(result.Rows[i][9]), journal = Convert.ToString(result.Rows[i][10]), journalName = Convert.ToString(result.Rows[i][11]), language = Convert.ToString(result.Rows[i][12]), year = (DateTime)(result.Rows[i][13]), issue = Convert.ToInt32(result.Rows[i][14]), page = Convert.ToInt32(result.Rows[i][15]), doi = Convert.ToString(result.Rows[i][16]), pdfFile = result.Rows[i][17].ToString(), comment = Convert.ToString(result.Rows[i][18]), status = Convert.ToString(result.Rows[i][19]) });
            }
            //listResult = WebHomeController.ConvertToList<InternationalPaper>(result);

            return Json(listResult, JsonRequestBehavior.AllowGet);
        }

        
    }
}