
using PureTechnologyTask.Models;
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace PureTechnologyTask.Controllers
{
    public class ResturentController : Controller
    {
        private readonly IConfiguration _configuration;

        public ResturentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult resturentmenu()
        {
            return View();
        }

        public IActionResult ordernow()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ordernow(FoodOrder f)
        {
            if (!ModelState.IsValid)
                return View(f);

            try
            {
                string path = _configuration.GetConnectionString("DefaultConnection");

                using (SqlConnection sc = new SqlConnection(path))
                {
                    sc.Open();

                    string orderQuery = @"
                        INSERT INTO FoodOrder (customerName, MenuType, TotalPrice, Description)
                        VALUES (@customerName, @MenuType, @TotalPrice, @Description);
                        SELECT CAST(SCOPE_IDENTITY() AS INT);";

                    int orderId;

                    using (SqlCommand cmd = new SqlCommand(orderQuery, sc))
                    {
                        cmd.Parameters.Add("@customerName", SqlDbType.NVarChar, 100).Value = f.customerName;
                        cmd.Parameters.Add("@MenuType", SqlDbType.NVarChar, 50).Value =
                            (object?)f.MenuType ?? DBNull.Value;
                        cmd.Parameters.Add("@TotalPrice", SqlDbType.Decimal).Value = f.TotalPrice;
                        cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 500).Value =
                            (object?)f.Description ?? DBNull.Value;

                        orderId = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    if (f.OrderItems != null)
                    {
                        foreach (OrderItem item in f.OrderItems)
                        {
                            if (item.Selected)
                            {
                                string itemQuery = @"
                                    INSERT INTO FoodOrderItems
                                    (OrderId, FoodName, ItemType, Price, Quantity)
                                    VALUES
                                    (@OrderId, @FoodName, @ItemType, @Price, @Quantity);";

                                using (SqlCommand cmd = new SqlCommand(itemQuery, sc))
                                {
                                    cmd.Parameters.Add("@OrderId", SqlDbType.Int).Value = orderId;
                                    cmd.Parameters.Add("@FoodName", SqlDbType.NVarChar, 100).Value = item.FoodName;
                                    cmd.Parameters.Add("@ItemType", SqlDbType.NVarChar, 50).Value = item.ItemType;
                                    cmd.Parameters.Add("@Price", SqlDbType.Decimal).Value = item.Price;
                                    cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = item.Quantity;

                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }

                    ViewBag.Status = "Order placed successfully! Order ID: " + orderId;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Status = "Error: " + ex.Message;
            }

            return View(f);
        }
    }
}
