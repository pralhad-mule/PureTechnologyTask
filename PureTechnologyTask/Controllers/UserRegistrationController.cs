
using PureTechnologyTask.Models;
using System;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace PureTechnologyTask.Controllers
{
    public class UserRegistrationController : Controller
    {
        private readonly IConfiguration _configuration;

        public UserRegistrationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult UserReg()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UserReg(userRegister k)
        {
            if (string.IsNullOrWhiteSpace(k.name) ||
                string.IsNullOrWhiteSpace(k.email) ||
                string.IsNullOrWhiteSpace(k.mobile_no) ||
                string.IsNullOrWhiteSpace(k.gender) ||
                string.IsNullOrWhiteSpace(k.password))
            {
                ViewData["status"] = "❌ Please fill all required fields!";
                return View(k);
            }

            if (k.mobile_no.Length == 10 && k.mobile_no.All(char.IsDigit))
            {
            }
            else
            {
                ViewData["status"] = "❌ Mobile number must be exactly 10 digits!";
                return View(k);
            }

            if (k.password != k.confirm_password)
            {
                ViewData["status"] = "❌ Password does not match!";
                return View(k);
            }

            string path = _configuration.GetConnectionString("DefaultConnection");

            SqlConnection sc = new SqlConnection(path);
            sc.Open();

            ViewData["status"] = "connection successfully....";

            string insert = "insert into users(name,email,mobile_no,gender,date_of_birth,address,password,confirm_password)values('" +
                k.name + "' , '" + k.email + "','" + k.mobile_no + "', '" + k.gender + "' , '" +
                k.Date_of_birth + "' , '" + k.address + "' , '" + k.password + "' , '" +
                k.confirm_password + "' )";

            SqlCommand scom = new SqlCommand(insert, sc);
            scom.ExecuteNonQuery();

            sc.Close();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult userlogin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult userlogin(string email, string password)
        {
            string path = _configuration.GetConnectionString("DefaultConnection");

            SqlConnection sc = new SqlConnection(path);

            string querry = "select * from users where email=@email AND password=@password";

            SqlCommand cmd = new SqlCommand(querry, sc);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@password", password);

            sc.Open();

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                dr.Close();
                sc.Close();
                return RedirectToAction("Room", "Rooms");
            }
            else
            {
                ViewBag.Message = "Invalid Email or Password";
            }

            dr.Close();
            sc.Close();

            return View();
        }
    }
}
