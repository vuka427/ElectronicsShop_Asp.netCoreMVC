using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using WebAppTinhVanCat_aspnetcore.Data;
using WebAppTinhVanCat_aspnetcore.Models;
using WebAppTinhVanCat_aspnetcore.Models.Product;

namespace WebAppTinhVanCat_aspnetcore.Areas.Product.Controllers
{
    [Area("Product")]
    [Route("/order/manage/{action}/")]
    [Authorize(Roles = RoleName.Administrator)]
    public class OrderManageController : Controller
    {

        private readonly AppDbContext _context;

        public OrderManageController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index([FromQuery(Name = "p")] int currentPage, int pagesize)
        {
            var orders = _context.Orders.OrderByDescending(o => o.CreateDate);

            var totalOrder = await orders.CountAsync(); // tổng số hóa đơn
            if (pagesize <= 0) pagesize = 10;
            var countPages = (int)Math.Ceiling((double)totalOrder / pagesize); // tính số lượng trang  =  tổng số hóa đơn / số sản phẩm trên 1 trang

            if (currentPage > countPages)
                currentPage = countPages;
            if (currentPage < 1)
                currentPage = 1;

            var pagingmodel = new PagingModel()
            {
                currentpage = currentPage,
                countpages = countPages,
                generateUrl = (int? pagenumber) => Url.Action("Index", new { p = pagenumber, pagesize = pagesize }) //phát url với pagenumber và pagesize
            };

            ViewBag.PagingModel = pagingmodel;
            ViewBag.OrderIndex = (currentPage - 1) * pagesize;
            ViewBag.TotalOrder = totalOrder;

            var ListProductInPage = await orders.Skip((currentPage - 1) * pagesize) // bỏ qua nhưng hóa đơn của trang trước đó
                        .Take(pagesize)// Hóa đơn trang hiện tại
                        .Include(o => o.OrderItems)
                        .ToListAsync();

            return View(ListProductInPage);
        }

        [Route("/order/manage/orderdetail/{orderid:int}", Name = "orderdetail")]
        public async  Task<IActionResult> OrderDetail([FromRoute] int orderid)
        {
            var order = await _context.Orders.Where(od => od.OrderId == orderid).Include(o=>o.OrderItems).FirstOrDefaultAsync();
            if(order != null)
            {
                return View(order);
            }
            return NotFound("lổi không tìm thấy hóa đơn!");
        }

        [HttpPost]
        public IActionResult AccessOrderApi(int id)
        {

            var order = _context.Orders.FirstOrDefault(od => od.OrderId == id);
            if(order != null)
            {
                if(order.State == StateOrder.Received)
                {
                    order.State = StateOrder.Accept;
                    _context.Orders.Update(order);
                    _context.SaveChanges();
                    return Json(new { error = 0 });
                }
            }
            return Json(new { error = 1 });
        }

        [HttpPost]
        public IActionResult CancelOrderApi(int id, string reason)
        {

            var order = _context.Orders.FirstOrDefault(od => od.OrderId == id);
            if (order != null)
            {
                if (order.State == StateOrder.Received || order.State == StateOrder.Accept)
                {
                    order.State = StateOrder.ShopCancel;
                    if (reason != null)
                        if (reason.Count() > 256)
                            reason = reason.Substring(0, 256);
                    order.ShopCancelReason = reason;
                    _context.SaveChanges();
                    return Json(new { error = 0 });
                }

            }
            return Json(new { error = 1 });
        }
    }
}
