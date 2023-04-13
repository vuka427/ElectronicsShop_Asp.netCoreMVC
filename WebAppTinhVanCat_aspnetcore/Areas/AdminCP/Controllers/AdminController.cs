using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using WebAppTinhVanCat_aspnetcore.Data;
using WebAppTinhVanCat_aspnetcore.Models;

namespace WebAppTinhVanCat_aspnetcore.Areas.AdminCP.Controllers
{
    [Area("AdminCP")]
    [Authorize(Roles = RoleName.Administrator)]
    public class AdminController : Controller
    {

        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        [Route("/admincp/")]
        public IActionResult Index()
        {
            DateTime day = DateTime.Now;
            
            var Order = _context.Orders.ToList();

            ViewBag.totalorder = Order.Where(o => o.State == Models.Product.StateOrder.Accept).Count(); // tổng số đơn hàng

            ViewBag.totalprice = Order.Where(o => o.State == Models.Product.StateOrder.Accept && o.Finished.Month == day.Month && o.Finished.Year == day.Year).Sum(o=> o.Price) ; // doanh thu tháng
            ViewBag.totalUser = _context.Users.Count();
            ViewBag.totalProduct = _context.Products.Count();
            ViewBag.totalBlog = _context.Posts.Count();
            ViewBag.totalContact = _context.Contacts.Count();
            
            ViewBag.PieChartHT = Order.Where(o => o.State == Models.Product.StateOrder.Accept && o.CreateDate.Month == day.Month && o.CreateDate.Year == day.Year).Count();
            ViewBag.PieChartXL = Order.Where(o => o.State == Models.Product.StateOrder.Received && o.CreateDate.Month == day.Month && o.CreateDate.Year == day.Year).Count();
            ViewBag.PieChartBH = Order.Where(o => o.State == Models.Product.StateOrder.ShopCancel || o.State == Models.Product.StateOrder.CustomerCancel && o.CreateDate.Month == day.Month && o.CreateDate.Year == day.Year).Count();

            return View();
        }
    }
}
