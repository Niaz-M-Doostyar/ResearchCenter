using ResearchCenter.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ResearchCenter.Areas.User.Controllers
{
    public class HomeController : Controller
    {
        // GET: User/Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult UserProfile()
        {
            try
            {
                var id = Session["id"] as int?;
                SqlConnectionClass connection = new SqlConnectionClass();
                var dt = connection.Select("select * from Registration where id = " + id);
                return View(dt);
            }
            catch(Exception e)
            { return View(e); }
            
        }

        [HttpGet]
        public ActionResult EditUserProfile(int id)
        {
            SqlConnectionClass connection = new SqlConnectionClass();
            var dt = connection.Select("select * from Registration where id = " + id);
            return View(dt);
        }

        [HttpPost]
        public ActionResult EditUserProfile(string oldPassword,string password, int id)
        {
            SqlConnectionClass connection = new SqlConnectionClass();

            var check = connection.Select("select password from Registration where id = " + id);
            Object[] checkPassword = ConvertDataTableToSingleArray(check);

            if (checkPassword[0].ToString() == oldPassword)
            {
                string query = "update Registration set password = '" + password + "' where id = " + id;
                connection.Update(query);

                return RedirectToAction("Login", "WebHome", new { area = "" });
            }
            else
            {
                ViewBag.error = "please type the correct old password";
                return View();
            }
            
        }

        //Converting DataTable to single type array
        public static object[] ConvertDataTableToSingleArray(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return new object[0];
            }

            // Calculate the total number of elements
            int totalElements = dt.Rows.Count * dt.Columns.Count;

            // Create a single-dimensional array to hold all values
            var array = new object[totalElements];

            int index = 0;
            // Loop through each row and column to populate the array
            foreach (DataRow row in dt.Rows)
            {
                foreach (var item in row.ItemArray)
                {
                    array[index++] = item;
                }
            }

            return array;
        }
    }
}