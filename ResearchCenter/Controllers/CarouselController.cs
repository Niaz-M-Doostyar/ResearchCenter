using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ResearchCenter.Models;

namespace ResearchCenter.Controllers
{
    public class CarouselController : Controller
    {
        // GET: Carousel
        public ActionResult ShowCarousel()
        {
            SqlConnectionClass connection = new SqlConnectionClass();
            var dt = connection.Select("select * from Carousel order by id desc");
            return View(dt);
        }

        [HttpGet]
        public ActionResult AddCarousel()
        {
            return View(new Carousel());
        }

        [HttpPost]
        public ActionResult AddCarousel(Carousel carousel)
        {
            SqlConnectionClass connection = new SqlConnectionClass();

            string Name = Path.GetFileName(carousel.image.FileName);

            if (carousel.image != null && carousel.image.ContentLength > 0)
            {
                string fileName = Path.GetFileName(carousel.image.FileName);
                string filePath = Path.Combine(Server.MapPath("~/UploadedFiles"), fileName);
                carousel.image.SaveAs(filePath);
            }
            string query = string.Format("INSERT INTO Carousel(description,image) VALUES('{0}','{1}')", carousel.description, Name);
            connection.Insert(query);

            return RedirectToAction("ShowCarousel");
        }

        [HttpGet]
        public ActionResult EditCarousel(int id)
        {
            SqlConnectionClass connection = new SqlConnectionClass();
            var dt = connection.Select("select * from Carousel where id = " + id);
            return View(dt);
        }

        [HttpPost]
        public ActionResult EditCarousel(Carousel carousel, int id)
        {
            SqlConnectionClass connection = new SqlConnectionClass();

            if (carousel.image != null && carousel.image.ContentLength > 0)
            {
                string fileName = Path.GetFileName(carousel.image.FileName);
                string filePath = Path.Combine(Server.MapPath("~/UploadedFiles"), fileName);
                carousel.image.SaveAs(filePath);

                string query = "update Carousel set image = '" + fileName + "' where id = " + id;
                connection.Insert(query);
            }

            string query1 = "update Carousel set description = '" + carousel.description + "' where id = " + id;
            connection.Insert(query1);

            return RedirectToAction("ShowCarousel");
        }

        [HttpGet]
        public ActionResult DeleteCarousel(int id)
        {
            SqlConnectionClass connection = new SqlConnectionClass();
            connection.Delete("delete from Carousel where id = " + id);
            return RedirectToAction("ShowCarousel");
        }
    }
}