using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ResearchCenter.Models;
//using System.Net;
//using System.Net.Mail;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;

namespace ResearchCenter.Controllers
{
    public class WebHomeController : Controller
    {
        // GET: Home
        public ActionResult Home()
        {
            SqlConnectionClass connection = new SqlConnectionClass();
            var dt = connection.Select("SELECT distinct Year(year) FROM NationalPaper");
            List<string> years = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][0] != DBNull.Value)
                {
                    years.Add(Convert.ToString(dt.Rows[i][0]));
                }
            }

            var dt1 = connection.Select("SELECT distinct Year(year) FROM InternationalPaper");
            List<string> internationalYears = new List<string>();
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                if (dt1.Rows[i][0] != DBNull.Value)
                {
                    internationalYears.Add(Convert.ToString(dt1.Rows[i][0]));
                }
            }

            var services = connection.Select("select TOP 3 * from Service order by id desc");
            List<Service> serviceTable = new List<Service>();
            for(int i = 0; i < services.Rows.Count; i++)
            {
                serviceTable.Add(new Service() { id = i, subject = Convert.ToString(services.Rows[i][1]), description = Convert.ToString(services.Rows[i][2]), imageString = services.Rows[i][3].ToString()});
            }

            var achievement = connection.Select("select TOP 3 * from Achievement order by id desc");
            List<Achievement> achievementTable = new List<Achievement>();
            for (int i = 0; i < achievement.Rows.Count; i++)
            {
                achievementTable.Add(new Achievement() { id = i, title = Convert.ToString(achievement.Rows[i][1]), description = Convert.ToString(achievement.Rows[i][2]), imageString = achievement.Rows[i][3].ToString() });
            }

            var carousel = connection.Select("select TOP 6 * from Carousel order by id desc");
            List<Carousel> carouselTable = new List<Carousel>();
            for(int i = 0; i < carousel.Rows.Count; i++)
            {
                carouselTable.Add(new Carousel() { id = i, description = Convert.ToString(carousel.Rows[i][1]), imageString = carousel.Rows[i][2].ToString() });
            }

            ViewBag.InternationalYears = internationalYears;
            ViewBag.Year = years;
            ViewBag.ServiceTable = serviceTable;
            ViewBag.AchievementTable = achievementTable;
            ViewBag.CarouselTable = carouselTable;

            return View();
        }

        public ActionResult PredatoryJournal()
        {
            SqlConnectionClass connection = new SqlConnectionClass();
            var dt = connection.Select("select * from PredatoryJournal order by id desc");
            return View(dt);
        }

        public ActionResult AcceptableJournal()
        {
            SqlConnectionClass connection = new SqlConnectionClass();
            var acceptJournals = connection.Select("select * from AcceptableJournal order by id desc");
            List<AcceptableJournal> acceptJournalsTable = new List<AcceptableJournal>();
            for (int i = 0; i < acceptJournals.Rows.Count; i++)
            {
                acceptJournalsTable.Add(new AcceptableJournal() { id = i, logoString = acceptJournals.Rows[i][1].ToString() });
            }

            ViewBag.AcceptJournal = acceptJournalsTable;
            return View();
        }

        public ActionResult Index()
        {
            SqlConnectionClass connection = new SqlConnectionClass();
            string query = " SELECT COUNT(*) AS TotalRecords FROM NationalPaper where status = 'Confirmed'";
            var result = connection.Select(query);
            List<string> countOfNationalPaper = new List<string>();
            for (int i = 0; i < result.Rows.Count; i++)
            {
                if (result.Rows[i][0] != DBNull.Value)
                {
                    countOfNationalPaper.Add(Convert.ToString(result.Rows[i][0]));
                }
            }

            string query1 = " SELECT COUNT(*) AS TotalRecords FROM InternationalPaper where status = 'Confirmed'";
            var result1 = connection.Select(query1);
            List<string> countOfInternationalPaper = new List<string>();
            for (int i = 0; i < result1.Rows.Count; i++)
            {
                if (result1.Rows[i][0] != DBNull.Value)
                {
                    countOfInternationalPaper.Add(Convert.ToString(result1.Rows[i][0]));
                }
            }

            ViewBag.International = countOfInternationalPaper;
            ViewBag.National = countOfNationalPaper;

            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View(new Registration());
        }

        //Login section for both admin and user side
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            SqlConnectionClass connection = new SqlConnectionClass();
            var status = connection.Select("select status from Registration where email = '" + email + "'");
            Object[] checkStatus = ConvertDataTableToSingleArray(status);
            Object checkedStatus = checkStatus[0];
            if(checkedStatus.ToString() != "Confirmed")
            {
                ViewBag.Message = "Your Account is not Verified Yet... Please try after some time";
                return View();
            }
            DataTable dt = new DataTable();
            dt = connection.Select("select * from Registration where email = '" + email + "' and password = '" + password + "'");
            List<Registration> RegTable = new List<Registration>();
            RegTable = ConvertToList<Registration>(dt);

            if (RegTable.Any())
            {
                if (RegTable.Any(r => r.email.Equals("lali.wais.kdr@gmail.com", StringComparison.OrdinalIgnoreCase)))
                {
                    if (RegTable.Any(r => r.password.Equals(password, StringComparison.OrdinalIgnoreCase)))
                    {
                        //add session
                        Session["id"] = RegTable.FirstOrDefault().id;
                        Session["name"] = RegTable.FirstOrDefault().name;
                        Session["username"] = RegTable.FirstOrDefault().email;
                        return RedirectToAction("Index", "WebHome");
                    }
                    else
                    {
                        ViewBag.error = "UserName of Password is wrong";
                        return View();
                    }
                }
                if(RegTable.Any(r => r.email.Equals(email, StringComparison.OrdinalIgnoreCase)))
                {
                    if(RegTable.Any(r => r.password.Equals(password, StringComparison.OrdinalIgnoreCase)))
                    {
                        //add session
                        Session["id"] = RegTable.FirstOrDefault().id;
                        Session["name"] = RegTable.FirstOrDefault().name;
                        Session["username"] = RegTable.FirstOrDefault().email;
                        return RedirectToAction("Index", "Home", new { area = "User" });
                    }
                    else
                    {
                        ViewBag.error = "UserName of Password is wrong";
                        return View();
                    }
                }
            }
            else
            {
                ViewBag.error = "UserName of Password is wrong";
                return View();
            }
            return View();

        }

        //Registration section for users
        [HttpGet]
        public ActionResult Registration()
        {
            return View(new Registration());
        }

        [HttpPost]
        public ActionResult Registration(Registration register)
        {
            DataTable dt = new DataTable();
            SqlConnectionClass con = new SqlConnectionClass();
            dt = con.Select("select * from Registration where email = '" + register.email + "'");

            List<Registration> reg = new List<Registration>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                reg.Add(new Registration() { id = Convert.ToInt32(dt.Rows[i][0]), name = Convert.ToString(dt.Rows[i][1]), phone = Convert.ToString(dt.Rows[i][2]), designation = Convert.ToString(dt.Rows[i][3]), department = Convert.ToString(dt.Rows[i][4]), faculty = Convert.ToString(dt.Rows[i][5]), email = Convert.ToString(dt.Rows[i][6]), password = Convert.ToString(dt.Rows[i][7]), status = Convert.ToString(dt.Rows[i][8]) });
            }

                if(reg.Any(r => r.email.Equals(register.email, StringComparison.OrdinalIgnoreCase)))
                {
                    ViewBag.error = "Username already exists";
                    return View();
                }
                else
                {
                    SqlConnectionClass connection = new SqlConnectionClass();
                    string status = "pending";
                    string query = string.Format("INSERT INTO Registration(name,phone,designation,department,faculty,email,password,status) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", register.name, register.phone, register.designation, register.department, register.faculty, register.email, register.password, status);
                    connection.Insert(query);
                    return RedirectToAction("Login", "WebHome"); ;
                }

            

        }

        [HttpGet]
        public ActionResult UserSelection()
        {
            SqlConnectionClass connection = new SqlConnectionClass();
            var dt = connection.Select("select * from Registration");
            return View(dt);
        }

        [HttpGet]
        public ActionResult EditUser(int id)
        {
            SqlConnectionClass connection = new SqlConnectionClass();
            var result = connection.Select("select * from Registration where id = " + id);
            return View(result);
        }

        [HttpPost]
        public ActionResult EditUser(Registration register, int id)
        {
            SqlConnectionClass connection = new SqlConnectionClass();
            string query = "update Registration set name = '" + register.name + "', phone = '" + register.phone + "', designation = '" + register.designation + "', department = '" + register.department + "', faculty = '" + register.faculty + "', email = '" + register.email + "', password = '" + register.password + "' where id = " + id;
            connection.Update(query);
            return RedirectToAction("UserSelection");
        }

        public JsonResult Search(string query)
        {
            SqlConnectionClass connection = new SqlConnectionClass();
            string q = "SELECT * FROM Registration WHERE name = '" + query + "' OR designation = '" + query + "'";
            DataTable result = new DataTable();
            result = connection.Select(q);

            List<Registration> listResult = new List<Registration>();
            for (int i = 0; i < result.Rows.Count; i++)
            {
                listResult.Add(new Registration() { id = Convert.ToInt32(result.Rows[i][0]), name = Convert.ToString(result.Rows[i][1]), designation = Convert.ToString(result.Rows[i][2]), department = Convert.ToString(result.Rows[i][3]), faculty = Convert.ToString(result.Rows[i][4]), phone = Convert.ToString(result.Rows[i][5]), email = Convert.ToString(result.Rows[i][6]), password = Convert.ToString(result.Rows[i][7]), status = Convert.ToString(result.Rows[i][8]) });
            }
            //listResult = WebHomeController.ConvertToList<InternationalPaper>(result);

            return Json(listResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DeleteUser(int id)
        {
            SqlConnectionClass connection = new SqlConnectionClass();
            connection.Delete("delete from Registration where id = " + id);
            return RedirectToAction("UserSelection");
        }

        [HttpGet]
        //public ActionResult ConfirmUser(int id, string status)
        //{
        //    SqlConnectionClass connection = new SqlConnectionClass();
        //    if (status == "1")
        //    {
        //        var dt = connection.Select("select email from Registration where id = " + id);
        //        Object[] userEmail = ConvertDataTableToSingleArray(dt);
        //        string user = "lali.wais.kdr@gmail.com";
        //        //string subject = "Confirmation Email";
        //        //string body = "Hello sir, your account with Research Center has been verified. Please login to Research Center system and go with further process. Thank you";
        //        //MailAddress fromAddress = new MailAddress("zubair.basiri@gmail.com", "Mail Support");
        //        //MailAddress toAddress = new MailAddress("lali.wais.kdr@gmail.com", "Dear Customer");
        //        //const string fromPassword = "tgcjzswsnjikoggo";
        //        //SmtpClient smtpClient = new SmtpClient();
        //        //smtpClient.UseDefaultCredentials = false;
        //        //smtpClient.Credentials = new NetworkCredential(fromAddress.Address, fromPassword);
        //        //smtpClient.Host = "smtp.gmail.com";
        //        //smtpClient.Port = 587;
        //        //smtpClient.EnableSsl = true;
        //        //smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        //        //MailMessage mailMessage = new MailMessage(fromAddress.Address, toAddress.Address, subject, body);
        //        //MailMessage mm = new MailMessage();
        //        //mm.From = new MailAddress("zubair.basiri@gmail.com");
        //        //mm.Subject = "Confirmation Email";
        //        //mm.Body = "Hello sir, your account with Research Center has been verified. Please login to Research Center system and go with further process. Thank you";
        //        //mm.To.Add(new MailAddress("lali.wais.kdr@gmail.com"));
        //        //mm.IsBodyHtml = false;

        //        //SmtpClient smtp = new SmtpClient();
        //        //smtp.Host = "smtp.gmail.com";
        //        //smtp.Port = 587;
        //        //smtp.UseDefaultCredentials = false;
        //        //smtp.EnableSsl = true;
        //        //NetworkCredential nc = new NetworkCredential("zubair.basiri@gmail.com", "zubair@704");
        //        //smtp.Credentials = nc;

        //        var smtpClient = new SmtpClient
        //        {
        //            Host = "smtp.gmail.com",
        //            Port = 587,              // For TLS
        //            EnableSsl = true,        // Enable SSL/TLS
        //            DeliveryMethod = SmtpDeliveryMethod.Network,
        //            UseDefaultCredentials = false,
        //            Credentials = new NetworkCredential("zubair.basiri@gmail.com", "sbrxygdvbdrtxif") // Use App Password here
        //        };

        //        var mailMessage = new MailMessage
        //        {
        //            From = new MailAddress("zubair.basiri@gmail.com"),
        //            Subject = "Confirmation Email",
        //            Body = "Hello sir, your account with Research Center has been verified. Please login to Research Center system and go with further process. Thank you",
        //            IsBodyHtml = true
        //        };

        //        mailMessage.To.Add("niaz.doostyar786@gmail.com");

        //        try
        //        {
        //            smtpClient.Send(mailMessage);
        //            return Content("Email sent successfully!");
        //            //return RedirectToAction("UserSelection");
        //        }
        //        catch (Exception ex)
        //        {
        //            // Handle the exception
        //            return Content("Error sending email: " + ex.Message);
        //        }
        //        //smtpClient.Send(mailMessage);
        //        //ViewBag.Message = "Ther user notified";
        //        //connection.Update("update Registration set status = 'Confirmed' where id = " + id);
        //        //}
        //        //catch (Exception ex)
        //        //{
        //        //    ViewBag.Message = $"Error in notifying, might be the email of the user is wrong: {ex.Message}";
        //        //}
        //        return RedirectToAction("UserSelection");
        //    }
        //    else
        //    {
        //        connection.Update("update Registration set status = 'Rejected' where id = " + id);
        //        return RedirectToAction("UserSelection");
        //    }

        //}

        //Sending Confirmation email to the user
        public async Task<ActionResult> SendEmail(int id, string status)
        {
            SqlConnectionClass connection = new SqlConnectionClass();
            if (status == "1")
            {
                var dt = connection.Select("select email, name from Registration where id = " + id);
                Object[] userEmail = ConvertDataTableToSingleArray(dt);
                Object user = userEmail[0].ToString();
                Object userName = userEmail[1].ToString();
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("WM. Lali", "lali.wais.kdr@gmail.com"));
                message.To.Add(new MailboxAddress(userName.ToString(), user.ToString()));
                message.Subject = "Confirmation Email";

                message.Body = new TextPart("plain")
                {
                    Text = "Dear " + userName + ",\n\nYour account with Research Center has been verified. Please login to Research Center system and go with further process.\nThank you\n\nBest Regards,\nLali"
                };

                using (var client = new SmtpClient())
                {
                    try
                    {
                        // For demo purposes, accept all SSL certificates (not recommended for production)
                        client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                        // Connect to Gmail SMTP server
                        await client.ConnectAsync("smtp.gmail.com", 587, false);

                        // Authenticate with the App Password
                        await client.AuthenticateAsync("lali.wais.kdr@gmail.com", "rxuflzcmocuexxog");

                        // Send the email
                        await client.SendAsync(message);

                        // Disconnect and quit
                        await client.DisconnectAsync(true);

                        connection.Update("update Registration set status = 'Confirmed' where id = " + id);
                        return RedirectToAction("UserSelection");
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions
                        return Content("Error sending email: " + ex.Message);
                    }
                }
            }
            else
            {
                connection.Update("update Registration set status = 'Rejected' where id = " + id);
                return RedirectToAction("UserSelection");
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Login", "WebHome");
        }

        //For generating 2d charts for National Papers
        [HttpGet]
        public ActionResult GetChartByFaculty()
        {
            SqlConnectionClass connection = new SqlConnectionClass();
            string query = "SELECT COUNT(*) AS TotalRecords FROM NationalPaper WHERE YEAR(year) = '2024' AND status = 'Confirmed' GROUP BY faculty ORDER BY faculty";
            string query1 = "SELECT distinct faculty from NationalPaper where Year(year) = '2024'";
            var dt = connection.Select(query);
            var dt1 = connection.Select(query1);
            object[] labelArray = ConvertDataTableToSingleArray(dt1);
            object[] dataArray = ConvertDataTableToSingleArray(dt);

            var data = new
            {
                labels = labelArray,
                datasets = new[]
                {
                new
                {
                    label = "P as Papers",
                    data = dataArray,
                    backgroundColor = new[]
                    {
                        "rgb(132, 49, 1)",
                        "rgb(66, 68, 237)",
                        "rgb(200, 14, 73)",
                        "rgb(75, 26, 141)",
                        "rgb(58, 130, 20)",
                        "rgb(106, 3, 2)",
                        "rgb(86, 40, 155)",
                        "rgb(239, 188, 17)",
                        "rgb(11, 183, 244)",
                        "rgb(242, 71, 170)",
                        "rgb(82, 205, 23)"
                    },
                    borderColor = new[]
                    {
                        "rgb(132, 49, 1)",
                        "rgb(66, 68, 237)",
                        "rgb(200, 14, 73)",
                        "rgb(75, 26, 141)",
                        "rgb(58, 130, 20)",
                        "rgb(106, 3, 2)",
                        "rgb(86, 40, 155)",
                        "rgb(239, 188, 17)",
                        "rgb(11, 183, 244)",
                        "rgb(242, 71, 170)",
                        "rgb(82, 205, 23)"
                    },
                    borderWidth = 1
                }
            }
            };

            return Json(data, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GetChartByFaculty(string inputData)
        {

            SqlConnectionClass connection = new SqlConnectionClass();
            string query = "SELECT COUNT(*) AS TotalRecords FROM NationalPaper WHERE YEAR(year) = '" + inputData + "' AND status = 'Confirmed' GROUP BY faculty ORDER BY faculty";
            string query1 = "SELECT distinct faculty from NationalPaper where Year(year) = '" + inputData + "' AND status = 'Confirmed'";
            var dt = connection.Select(query);
            var dt1 = connection.Select(query1);
            object[] labelArray = ConvertDataTableToSingleArray(dt1);
            object[] dataArray = ConvertDataTableToSingleArray(dt);

            var data = new
            {
                labels = labelArray,
                datasets = new[]
                {
                new
                {
                    label = "P as Papers",
                    data = dataArray,
                    backgroundColor = new[]
                    {
                        "rgb(132, 49, 1)",
                        "rgb(66, 68, 237)",
                        "rgb(200, 14, 73)",
                        "rgb(75, 26, 141)",
                        "rgb(58, 130, 20)",
                        "rgb(106, 3, 2)",
                        "rgb(86, 40, 155)",
                        "rgb(239, 188, 17)",
                        "rgb(11, 183, 244)",
                        "rgb(242, 71, 170)",
                        "rgb(82, 205, 23)"
                    },
                    borderColor = new[]
                    {
                        "rgb(132, 49, 1)",
                        "rgb(66, 68, 237)",
                        "rgb(200, 14, 73)",
                        "rgb(75, 26, 141)",
                        "rgb(58, 130, 20)",
                        "rgb(106, 3, 2)",
                        "rgb(86, 40, 155)",
                        "rgb(239, 188, 17)",
                        "rgb(11, 183, 244)",
                        "rgb(242, 71, 170)",
                        "rgb(82, 205, 23)"
                    },
                    borderWidth = 1
                }
            }
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetChartByDesignation()
        {
            SqlConnectionClass connection = new SqlConnectionClass();
            string query = "SELECT COUNT(*) AS TotalRecords FROM NationalPaper WHERE YEAR(year) = '2024' AND status = 'Confirmed' GROUP BY designation ORDER BY designation";
            string query1 = "SELECT distinct designation from NationalPaper where Year(year) = '2024' AND status = 'Confirmed'";
            var dt = connection.Select(query);
            var dt1 = connection.Select(query1);
            object[] labelArray = ConvertDataTableToSingleArray(dt1);
            object[] dataArray = ConvertDataTableToSingleArray(dt);
            var data = new
            {
                labels = labelArray,
                datasets = new[]
                {
                new
                {
                    label = "P as Papers",
                    data = dataArray,
                    backgroundColor = new[]
                    {
                        "rgb(30, 73, 191)",
                        "rgb(255, 151, 2)",
                        "rgb(254, 0, 26)",
                        "rgb(2, 161, 225)",
                        "rgb(183, 0, 90)",
                    },
                    borderColor = new[]
                    {
                        "rgb(30, 73, 191)",
                        "rgb(255, 151, 2)",
                        "rgb(254, 0, 26)",
                        "rgb(2, 161, 225)",
                        "rgb(183, 0, 90)",
                    },
                    borderWidth = 1
                }
            }
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetChartByDesignation(string input)
        {
            SqlConnectionClass connection = new SqlConnectionClass();
            string query = "SELECT COUNT(*) AS TotalRecords FROM NationalPaper WHERE YEAR(year) = '" + input + "' AND status = 'Confirmed' GROUP BY designation ORDER BY designation";
            string query1 = "SELECT distinct designation from NationalPaper where Year(year) = '" + input + "' AND status = 'Confirmed'";
            var dt = connection.Select(query);
            var dt1 = connection.Select(query1);
            object[] labelArray = ConvertDataTableToSingleArray(dt1);
            object[] dataArray = ConvertDataTableToSingleArray(dt);
            var data = new
            {
                labels = labelArray,
                datasets = new[]
                {
                new
                {
                    label = "P as Papers",
                    data = dataArray,
                    backgroundColor = new[]
                    {
                        "rgb(30, 73, 191)",
                        "rgb(255, 151, 2)",
                        "rgb(254, 0, 26)",
                        "rgb(2, 161, 225)",
                        "rgb(183, 0, 90)",
                    },
                    borderColor = new[]
                    {
                        "rgb(30, 73, 191)",
                        "rgb(255, 151, 2)",
                        "rgb(254, 0, 26)",
                        "rgb(2, 161, 225)",
                        "rgb(183, 0, 90)",
                    },
                    borderWidth = 1
                }
            }
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetChartByFacultyInternational()
        {
            SqlConnectionClass connection = new SqlConnectionClass();
            string query = "SELECT COUNT(*) AS TotalRecords FROM InternationalPaper WHERE YEAR(year) = '2024' AND status = 'Confirmed' GROUP BY faculty ORDER BY faculty";
            string query1 = "SELECT distinct faculty from InternationalPaper where Year(year) = '2024' AND status = 'Confirmed'";
            var dt = connection.Select(query);
            var dt1 = connection.Select(query1);
            object[] labelArray = ConvertDataTableToSingleArray(dt1);
            object[] dataArray = ConvertDataTableToSingleArray(dt);

            var data = new
            {
                labels = labelArray,
                datasets = new[]
                {
                new
                {
                    label = "P as Papers",
                    data = dataArray,
                    backgroundColor = new[]
                    {
                        "rgb(132, 49, 1)",
                        "rgb(66, 68, 237)",
                        "rgb(200, 14, 73)",
                        "rgb(75, 26, 141)",
                        "rgb(58, 130, 20)",
                        "rgb(106, 3, 2)",
                        "rgb(86, 40, 155)",
                        "rgb(239, 188, 17)",
                        "rgb(11, 183, 244)",
                        "rgb(242, 71, 170)",
                        "rgb(82, 205, 23)"
                    },
                    borderColor = new[]
                    {
                        "rgb(132, 49, 1)",
                        "rgb(66, 68, 237)",
                        "rgb(200, 14, 73)",
                        "rgb(75, 26, 141)",
                        "rgb(58, 130, 20)",
                        "rgb(106, 3, 2)",
                        "rgb(86, 40, 155)",
                        "rgb(239, 188, 17)",
                        "rgb(11, 183, 244)",
                        "rgb(242, 71, 170)",
                        "rgb(82, 205, 23)"
                    },
                    borderWidth = 1
                }
            }
            };

            return Json(data, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GetChartByFacultyInternational(string inputYear)
        {

            SqlConnectionClass connection = new SqlConnectionClass();
            string query = "SELECT COUNT(*) AS TotalRecords FROM InternationalPaper WHERE YEAR(year) = '" + inputYear + "' AND status = 'Confirmed' GROUP BY faculty ORDER BY faculty";
            string query1 = "SELECT distinct faculty from InternationalPaper where Year(year) = '" + inputYear + "' AND status = 'Confirmed'";
            var dt = connection.Select(query);
            var dt1 = connection.Select(query1);
            object[] labelArray = ConvertDataTableToSingleArray(dt1);
            object[] dataArray = ConvertDataTableToSingleArray(dt);

            var data = new
            {
                labels = labelArray,
                datasets = new[]
                {
                new
                {
                    label = "P as Papers",
                    data = dataArray,
                    backgroundColor = new[]
                    {
                        "rgb(132, 49, 1)",
                        "rgb(66, 68, 237)",
                        "rgb(200, 14, 73)",
                        "rgb(75, 26, 141)",
                        "rgb(58, 130, 20)",
                        "rgb(106, 3, 2)",
                        "rgb(86, 40, 155)",
                        "rgb(239, 188, 17)",
                        "rgb(11, 183, 244)",
                        "rgb(242, 71, 170)",
                        "rgb(82, 205, 23)"
                    },
                    borderColor = new[]
                    {
                        "rgb(132, 49, 1)",
                        "rgb(66, 68, 237)",
                        "rgb(200, 14, 73)",
                        "rgb(75, 26, 141)",
                        "rgb(58, 130, 20)",
                        "rgb(106, 3, 2)",
                        "rgb(86, 40, 155)",
                        "rgb(239, 188, 17)",
                        "rgb(11, 183, 244)",
                        "rgb(242, 71, 170)",
                        "rgb(82, 205, 23)"
                    },
                    borderWidth = 1
                }
            }
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetChartByDesignationInternational()
        {
            SqlConnectionClass connection = new SqlConnectionClass();
            string query = "SELECT COUNT(*) AS TotalRecords FROM InternationalPaper WHERE YEAR(year) = '2024' AND status = 'Confirmed' GROUP BY designation ORDER BY designation";
            string query1 = "SELECT distinct designation from InternationalPaper where Year(year) = '2024' AND status = 'Confirmed'";
            var dt = connection.Select(query);
            var dt1 = connection.Select(query1);
            object[] labelArray = ConvertDataTableToSingleArray(dt1);
            object[] dataArray = ConvertDataTableToSingleArray(dt);
            var data = new
            {
                labels = labelArray,
                datasets = new[]
                {
                new
                {
                    label = "P as Papers",
                    data = dataArray,
                    backgroundColor = new[]
                    {
                        "rgb(30, 73, 191)",
                        "rgb(255, 151, 2)",
                        "rgb(254, 0, 26)",
                        "rgb(2, 161, 225)",
                        "rgb(183, 0, 90)",
                    },
                    borderColor = new[]
                    {
                        "rgb(30, 73, 191)",
                        "rgb(255, 151, 2)",
                        "rgb(254, 0, 26)",
                        "rgb(2, 161, 225)",
                        "rgb(183, 0, 90)",
                    },
                    borderWidth = 1
                }
            }
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetChartByDesignationInternational(string inputYear1)
        {
            SqlConnectionClass connection = new SqlConnectionClass();
            string query = "SELECT COUNT(*) AS TotalRecords FROM InternationalPaper WHERE YEAR(year) = '" + inputYear1 + "' AND status = 'Confirmed' GROUP BY designation ORDER BY designation";
            string query1 = "SELECT distinct designation from InternationalPaper where Year(year) = '" + inputYear1 + "' AND status = 'Confirmed'";
            var dt = connection.Select(query);
            var dt1 = connection.Select(query1);
            object[] labelArray = ConvertDataTableToSingleArray(dt1);
            object[] dataArray = ConvertDataTableToSingleArray(dt);
            var data = new
            {
                labels = labelArray,
                datasets = new[]
                {
                new
                {
                    label = "P as Papers",
                    data = dataArray,
                    backgroundColor = new[]
                    {
                        "rgb(30, 73, 191)",
                        "rgb(255, 151, 2)",
                        "rgb(254, 0, 26)",
                        "rgb(2, 161, 225)",
                        "rgb(183, 0, 90)",
                    },
                    borderColor = new[]
                    {
                        "rgb(30, 73, 191)",
                        "rgb(255, 151, 2)",
                        "rgb(254, 0, 26)",
                        "rgb(2, 161, 225)",
                        "rgb(183, 0, 90)",
                    },
                    borderWidth = 1
                }
            }
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetChartByYearInternational()
        {
            SqlConnectionClass connection = new SqlConnectionClass();
            string query = "SELECT COUNT(*) AS TotalRecords FROM InternationalPaper WHERE status = 'Confirmed' GROUP BY Year(year) ORDER BY Year(year)";
            string query1 = "SELECT distinct Year(year) from InternationalPaper where status = 'Confirmed'";
            var dt = connection.Select(query);
            var dt1 = connection.Select(query1);
            object[] labelArray = ConvertDataTableToSingleArray(dt1);
            object[] dataArray = ConvertDataTableToSingleArray(dt);
            //object[] newData = { 0, 0, 0, 0, 0 };

            //if (0 + labelArray.Length <= newData.Length)
            //{
            //    // Copy the smaller array into the larger array at the specified index
            //    Array.Copy(labelArray, 0, newData, 0, labelArray.Length);
            //}
            var data = new
            {
                labels = labelArray,
                datasets = new[]
                {
                new
                {
                    label = "Published Papers",
                    data = dataArray,
                    backgroundColor = new[]
                    {
                        "rgba(255, 255, 255, 0.25)",
                    },
                    borderColor = new[]
                    {
                        "rgb(30, 73, 191)",
                        "rgb(255, 151, 2)",
                        "rgb(254, 0, 26)",
                        "rgb(2, 161, 225)",
                        "rgb(183, 0, 90)",
                        "rgb(132, 49, 1)",
                        "rgb(66, 68, 237)",
                        "rgb(200, 14, 73)",
                        "rgb(75, 26, 141)",
                        "rgb(58, 130, 20)",
                        "rgb(106, 3, 2)",
                        "rgb(86, 40, 155)",
                        "rgb(239, 188, 17)",
                        "rgb(11, 183, 244)",
                        "rgb(242, 71, 170)",
                        "rgb(82, 205, 23)"
                    },
                    borderWidth = 3,
                    tension = 0.4, // This makes the line smooth
                    pointRadius = 5,
                    fill = true
                }
            }
            };

            return Json(data, JsonRequestBehavior.AllowGet);
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

        //Converting DataTable to list
        public static List<T> ConvertToList<T>(DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName.ToLower()).ToList();
            var properties = typeof(T).GetProperties();
            return dt.AsEnumerable().Select(row => {
                var objT = Activator.CreateInstance<T>();
                foreach (var pro in properties)
                {
                    if (columnNames.Contains(pro.Name.ToLower()))
                    {
                        try
                        {
                            pro.SetValue(objT, row[pro.Name]);
                        }
                        catch (Exception ex) { }
                    }
                }
                return objT;
            }).ToList();
        }
    }
}