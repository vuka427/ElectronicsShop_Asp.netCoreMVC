﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System;
using WebAppTinhVanCat_aspnetcore.Models.Product;
using WebAppTinhVanCat_aspnetcore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp;
using Castle.Core.Resource;

namespace WebAppTinhVanCat_aspnetcore.Areas.Product.Controllers
{
    [Area("Product")]
    [Route("myorder/{action}/")]
    [Authorize]
    public class MyOrderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        [TempData]
        public string StatusMessage { get; set; }


        public MyOrderController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index([FromQuery(Name = "p")] int currentPage, int pagesize)
        {
            

            AppUser user = await GetAppUserAsync();

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            IQueryable<OrderModel> orders = null;

           
            
            orders = _context.Orders.Where(o=>o.CustomerID == user.Id).OrderByDescending(o => o.CreateDate);
            

            int totalOrder = 0;
            if (orders != null)
            {
                totalOrder = await orders.CountAsync(); // tổng số hóa đơn
            }



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

            var ListOrdertInPage = await orders.Skip((currentPage - 1) * pagesize) // bỏ qua nhưng hóa đơn của trang trước đó
                        .Take(pagesize)// Hóa đơn trang hiện tại
                        .Include(o => o.OrderItems)
                        .ToListAsync();

            return View(ListOrdertInPage);
        }

        public async Task<IActionResult> OrderDetail([FromQuery] string ordercode)
        {
            AppUser user = await GetAppUserAsync();

            if (user == null)
            {
                StatusMessage = "Error không tìm thấy người dùng ";
                return RedirectToAction("Index", "Home");
            }
            var order = await _context.Orders.Where(od => od.OrderCode == ordercode && od.CustomerID == user.Id).Include(o => o.OrderItems).FirstOrDefaultAsync();
            if (order != null)
            {
                return View(order);
            }
            StatusMessage = "Error không tìm thấy đơn hàng";
            return RedirectToAction("Index", "Home");
        }



        private async Task<AppUser> GetAppUserAsync()
        {
            return await _userManager.GetUserAsync(User);
        }


    }
}
