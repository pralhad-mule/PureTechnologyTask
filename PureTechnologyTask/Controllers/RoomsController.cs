using PureTechnologyTask.Models;
using System;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace PureTechnologyTask.Controllers
{
    public class RoomsController : Controller
    {
        public IActionResult Room()
        {
            return View();
        }

        public IActionResult view_details_standard()
        {
            return View();
        }

        public IActionResult view_details_delux()
        {
            return View();
        }

        public IActionResult view_details_superdelux()
        {
            return View();
        }

        public IActionResult view_details_family()
        {
            return View();
        }

        public IActionResult view_details_president()
        {
            return View();
        }

        public IActionResult view_details_metting()
        {
            return View();
        }

        public IActionResult view_details_executative()
        {
            return View();
        }

        public IActionResult view_details_premium()
        {
            return View();
        }

        public IActionResult view_details_luxary()
        {
            return View();
        }

        public IActionResult view_details_business()
        {
            return View();
        }

        public IActionResult book_room()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult book_room(Booking b)
        {
            if (string.IsNullOrWhiteSpace(b.custimername) ||
                string.IsNullOrWhiteSpace(b.email) ||
                string.IsNullOrWhiteSpace(b.phone) ||
                string.IsNullOrWhiteSpace(b.roomtype) ||
                string.IsNullOrWhiteSpace(b.CheckInDate) ||
                string.IsNullOrWhiteSpace(b.CheckOutDate) ||
                b.Guests <= 0 ||
                b.Price_Per_Nigh <= 0 ||
                b.TotalPrice <= 0)
            {
                ViewData["status"] = "Please fill in all the required fields.";
                return View(b);
            }

            if (!DateTime.TryParseExact(
                    b.CheckInDate, "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime checkIn) ||
                !DateTime.TryParseExact(
                    b.CheckOutDate, "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime checkOut) ||
                checkOut <= checkIn)
            {
                ViewData["status"] = "Please select valid check-in and check-out dates.";
                return View(b);
            }

            string path = HttpContext.RequestServices
                .GetRequiredService<IConfiguration>()
                .GetConnectionString("DefaultConnection");

            try
            {
                using (SqlConnection sc = new SqlConnection(path))
                {
                    sc.Open();

                    string insert = @"INSERT INTO roombook
                        (custimername, email, phone, roomtype, CheckInDate,
                         CheckOutDate, Guests, Price_Per_Nig, TotalPrice)
                        VALUES
                        (@name, @email, @phone, @roomtype, @checkin,
                         @checkout, @guests, @price, @total)";

                    using (SqlCommand scom = new SqlCommand(insert, sc))
                    {
                        scom.Parameters.AddWithValue("@name", b.custimername);
                        scom.Parameters.AddWithValue("@email", b.email);
                        scom.Parameters.AddWithValue("@phone", b.phone);
                        scom.Parameters.AddWithValue("@roomtype", b.roomtype);
                        scom.Parameters.AddWithValue("@checkin", checkIn);
                        scom.Parameters.AddWithValue("@checkout", checkOut);
                        scom.Parameters.AddWithValue("@guests", b.Guests);
                        scom.Parameters.AddWithValue("@price", b.Price_Per_Nigh);
                        scom.Parameters.AddWithValue("@total", b.TotalPrice);

                        int rows = scom.ExecuteNonQuery();

                        ViewData["status"] = rows > 0
                            ? "Booking Successfully Saved!"
                            : "Booking was not inserted.";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewData["status"] = "Database error: " + ex.Message;
            }

            return View(b);
        }
    }
}