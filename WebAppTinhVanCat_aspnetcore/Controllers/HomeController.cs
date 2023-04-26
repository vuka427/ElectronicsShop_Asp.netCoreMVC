using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebAppTinhVanCat_aspnetcore.Models;
using WebAppTinhVanCat_aspnetcore.Models.Blog;
using WebAppTinhVanCat_aspnetcore.Models.Product;
using WebAppTinhVanCat_aspnetcore.Models.ViewModelHome;

namespace WebAppTinhVanCat_aspnetcore.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            var qr = (from c in _context.CategoryProducts select c)
                                       .Include(c => c.ParentCategory)
                                       .Include(c => c.CategoryChildren);
            List<CategoryProduct> cates = (await qr.ToListAsync()).Where(c => c.ParentCategory == null).ToList();
            ViewBag.categories = cates;
            var products = _context.Products.Include(p => p.Photos).OrderByDescending(p => p.DateUpdated);
             
            List< HomeViewModel > listNewProduct = new List< HomeViewModel >();

            foreach (CategoryProduct c in cates)
            {
                var pd = new HomeViewModel();
                pd.Title = c.Title;
                pd.Slug = c.Slug;

                var idCateChill = new List<int>();
                c.ChildCategoryIDs(idCateChill, null);// lấy id của tất cả danh mục con
                idCateChill.Add(c.Id);


                pd.Products = products.Where(p => idCateChill.Contains(p.CategoryId)).Take(4).ToList();


                listNewProduct.Add(pd);
            }

            ViewBag.catepd = listNewProduct;



            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
       
    }
}
