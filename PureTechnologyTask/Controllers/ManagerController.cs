
using PureTechnologyTask.Models;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace PureTechnologyTask.Controllers
{
    public class ManagerController : Controller
    {
        private readonly IConfiguration _configuration;

        public ManagerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(manager m)
        {
            string path = _configuration.GetConnectionString("DefaultConnection");

            SqlConnection sc = new SqlConnection(path);
            sc.Open();

            ViewData["Status"] = "Connection successful!";

            string insert = "insert into Manager(name ,email,phone,password,branch)values('" +
                m.name + "',   '" + m.email + "','" + m.phone + "','" +
                m.password + "','" + m.Branch + "')";

            SqlCommand scom = new SqlCommand(insert, sc);
            scom.ExecuteNonQuery();

            sc.Close();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult managerlogin()
        {
            return View();
        }

        public IActionResult managerdashbord()
        {
            string path = _configuration.GetConnectionString("DefaultConnection");

            SqlConnection sc = new SqlConnection(path);
            sc.Open();

            SqlCommand cmd1 = new SqlCommand("SELECT COUNT(*) FROM users", sc);
            ViewBag.TotalUsers = cmd1.ExecuteScalar();

            SqlCommand cmd2 = new SqlCommand("SELECT COUNT(*) FROM roombook", sc);
            ViewBag.TotalBookings = cmd2.ExecuteScalar();

            List<Booking> roombook = new List<Booking>();

            SqlCommand cmd3 = new SqlCommand(
                "SELECT TOP 5 * FROM roombook ORDER BY Id DESC", sc);

            SqlDataReader dr = cmd3.ExecuteReader();

            while (dr.Read())
            {
                roombook.Add(new Booking()
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    custimername = dr["custimername"].ToString(),
                    roomtype = dr["roomtype"].ToString(),
                    CheckInDate = dr["CheckInDate"].ToString(),
                    CheckOutDate = dr["CheckOutDate"].ToString()
                });
            }

            dr.Close();
            sc.Close();

            return View(roombook);
        }

        public IActionResult User()
        {
            return View();
        }

        public IActionResult Room()
        {
            return View();
        }
    }
}
