using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ResearchCenter.Models;

namespace ResearchCenter.Controllers
{
    public class ServicesController : Controller
    {
        // GET: Services
        public ActionResult showServices()
        {
            SqlConnectionClass connection = new SqlConnectionClass();
            string query = "select * from Service decs order by id desc";
            var result = connection.Select(query);

            return View(result);
        }
        [HttpGet]
        public ActionResult CreateServices()
        {
            return View(new Service());
        }
        [HttpPost]
        public ActionResult CreateServices(Service service)
        {

            string Name = Path.GetFileName(service.image.FileName);

            if (service.image != null && service.image.ContentLength > 0)
            {
                string fileName = Path.GetFileName(service.image.FileName);
                string filePath = Path.Combine(Server.MapPath("~/UploadedFiles"), fileName);
                service.image.SaveAs(filePath);
            }

            SqlConnectionClass connection = new SqlConnectionClass();
            string query = string.Format("INSERT INTO Service(subject,description,image) VALUES('{0}','{1}','{2}')", service.subject, service.description, Name);
            connection.Insert(query);

            return RedirectToAction("showServices");
        }
        [HttpGet]
        public ActionResult EditServices(int id)
        {
            SqlConnectionClass connection = new SqlConnectionClass();
            var result = connection.Select("select * from Service where id = " + id);
            return View(result);
        }
        [HttpPost]
        public ActionResult EditServices(Service service, int id)
        {
            SqlConnectionClass connection = new SqlConnectionClass();

            if (service.image != null && service.image.ContentLength > 0)
            {
                string fileName = Path.GetFileName(service.image.FileName);
                string filePath = Path.Combine(Server.MapPath("~/UploadedFiles"), fileName);
                service.image.SaveAs(filePath);

                string query = "update Service set image = '" + fileName + "' where id = " + id;
                connection.Insert(query);
            }

            string query1 = "update Service set subject = '" + service.subject + "', description = '" + service.description + "' where id = " + id;
            connection.Insert(query1);

            return RedirectToAction("showServices");
        }

        public ActionResult DeleteServices(int id)
        {
            SqlConnectionClass connection = new SqlConnectionClass();
            string query = "delete from Service where id = " + id;
            connection.Delete(query);
            return RedirectToAction("showServices");
        }
    }
}