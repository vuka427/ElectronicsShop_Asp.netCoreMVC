using Castle.Core.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Evaluation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using WebAppTinhVanCat_aspnetcore.Areas.Product.Models;
using WebAppTinhVanCat_aspnetcore.Areas.Product.Service;
using WebAppTinhVanCat_aspnetcore.Models;
using WebAppTinhVanCat_aspnetcore.Models.Product;

namespace WebAppTinhVanCat_aspnetcore.Areas.Product.Controllers
{
    [Area("Product")]
    public class ViewProductController : Controller
    {
        private readonly ILogger<ViewProductController> _logger;
        private readonly AppDbContext _context;
        private readonly CartService _cartService;

        public ViewProductController(ILogger<ViewProductController> logger, AppDbContext context, CartService cartservice)
        {
            _logger = logger;
            _context = context;
            _cartService = cartservice;
        }



        //product
        //product/{productslug?}
        [Route("/product/{productslug?}")]
        [AllowAnonymous]
        public IActionResult Index(string productslug, [FromQuery(Name = "p")] int currentPage, int pagesize) 
        {
            ViewBag.categories = GetCategories();//tất cả danh mục
            ViewBag.categoryslug = productslug;//url truy cập hiện tại

            CategoryProduct category = null; //danh mục tương ứng với url truy cập hiện tại

            if (!string.IsNullOrEmpty(productslug) ) // nếu tồn tại url chỉ định danh mục thì lấy danh mục tương ứng
            {
                category = _context.CategoryProducts.Where(c=>c.Slug == productslug)
                                                .Include(c => c.CategoryChildren)                    
                                                .FirstOrDefault();
                if(category == null) return NotFound("không tìm thấy danh mục");
            }
            ViewBag.category = category;


            var product = _context.Products.Include(p => p.Author) //tất cả các sản phẩm
                                    .Include(p=>p.Photos)
                                    .Include(p => p.ProductCategoryProducts)
                                    .ThenInclude(p => p.Category)
                                    .AsQueryable(); 
                                    
            product = product.OrderByDescending(p => p.DateUpdated);


            if (category != null) //  nếu có danh mục thì lấy các sản phẩm con cháu và của danh mục đó
            {
                var ids = new List<int>();
                category.ChildCategoryIDs(ids, null);
                ids.Add(category.Id);

                product = product.Where(p => p.ProductCategoryProducts.Where(pc => ids.Contains(pc.CategoryProductID)).Any());
                
            }
            // phân trang
            
            var totalProduct =  product.Count(); // tổng số sản phẩm
            if (pagesize <= 0) pagesize = 12;
            var countPages = (int)Math.Ceiling((double)totalProduct / pagesize); // tính số lượng trang  =  tổng số sản phẩm / số sản phẩm trên 1 trang

            if (currentPage > countPages)
                currentPage = countPages;
            if (currentPage < 1)
                currentPage = 1;

            var pagingmodel = new PagingModel()
            {
                currentpage = currentPage,
                countpages = countPages,
                generateUrl = (int? pagenumber) => Url.Action("Index", new { p = pagenumber, pagesize = pagesize }) //action phát sinh url
            };

            ViewBag.PagingModel = pagingmodel;
            ViewBag.ProductIndex = (currentPage - 1) * pagesize;
            ViewBag.TotalProduct = totalProduct;

            var ListProductInPage = product.Skip((currentPage - 1) * pagesize) // bỏ qua những sản phẩm của trang trước đó
                        .Include(u=>u.UnitProduct)
                        .Take(pagesize)// lấy sản phẩm trang hiện tại
                        .ToList();
            

            return View(ListProductInPage);
        }




        [Route("/product/{productslug}.html")]
        [AllowAnonymous]
        public IActionResult Detail(string productslug) 
        {
            ViewBag.categories = GetCategories();//tất cả danh mục


            var Product = _context.Products.Where(p => p.Slug == productslug)
                                        .Include(p => p.Author)
                                        .Include(p => p.Photos)
                                        .Include(u=>u.UnitProduct)
                                        .Include(p => p.ProductCategoryProducts)
                                        .ThenInclude(pc => pc.Category)
                                        .FirstOrDefault();
            if(Product == null)
            {
                return NotFound("không thấy bài viết !");
            }
            CategoryProduct category =  Product.ProductCategoryProducts.FirstOrDefault()?.Category;//lấy danh mục của sản phẩm
            ViewBag.category = category;

            if (category != null)
            {
                var otherProduct = _context.Products.Where(p => p.ProductCategoryProducts.Any(c => c.Category.Id == category.Id)) // lấy 5 sản phẩm cùng danh mục
                                            .Where(p => p.ProductId != Product.ProductId)
                                            .OrderByDescending(p => p.DateUpdated)
                                            .Take(5);
                ViewBag.otherProduct = otherProduct;

            }
           
            

            return View(Product);
        }

        [NonAction]
        private List<CategoryProduct> GetCategories() // lấy tất cả cây danh mục  
        {

            var categories = _context.CategoryProducts
                                    .Include(c => c.CategoryChildren)
                                    .AsEnumerable()
                                    .Where(c=>c.ParentCategory == null)
                                    .ToList();

            return categories;

        }

       
    }
}
