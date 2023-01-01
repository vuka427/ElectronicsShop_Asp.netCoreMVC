using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using WebAppTinhVanCat_aspnetcore.Models;
using WebAppTinhVanCat_aspnetcore.Models.Products;

namespace WebAppTinhVanCat_aspnetcore.Areas.Product.Controllers
{
    [Area("Product")]
    public class ViewProductController : Controller
    {
        private readonly ILogger<ViewProductController> _logger;
        private readonly AppDbContext _context;

        public ViewProductController(ILogger<ViewProductController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        //product
        //post/{categoryslug?}
        [Route("/product/{categoryslug?}")]
        public IActionResult Index(string categoryslug, [FromQuery(Name = "p")] int currentPage, int pagesize) 
        {
            ViewBag.categories = GetCategories();//tất cả danh mục
            ViewBag.categoryslug = categoryslug;//url truy cập hiện tại

            CategoryProduct category = null; //danh mục tương ứng với url truy cập hiện tại

            if (!string.IsNullOrEmpty(categoryslug) ) // nếu tồn tại url chỉ định danh mục thì lấy danh mục tương ứng
            {
                category = _context.CategoryProducts.Where(c=>c.Slug == categoryslug)
                                                .Include(c => c.CategoryChildren)                    
                                                .FirstOrDefault();
                if(category == null) return NotFound("không tìm thấy danh mục");
            }
            ViewBag.category = category;


            var product = _context.Products.Include(p => p.Author) //tất cả các sản phẩm
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
            if (pagesize <= 0) pagesize = 10;
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
                        .Take(pagesize)// lấy sản phẩm trang hiện tại
                        .ToList();
            

            return View(ListProductInPage);
        }




        [Route("/product/{productslug}.html")]
        public IActionResult Detail(string productslug) 
        {
            ViewBag.categories = GetCategories();//tất cả danh mục


            var Post = _context.Products.Where(p => p.Slug == productslug)
                                        .Include(p => p.Author)
                                        .Include(p => p.ProductCategoryProducts)
                                        .ThenInclude(pc => pc.Category)
                                        .FirstOrDefault();
            if(Post == null)
            {
                return NotFound("không thấy bài viết !");
            }
            CategoryProduct category =  Post.ProductCategoryProducts.FirstOrDefault()?.Category;//lấy danh mục của sản phẩm
            ViewBag.category = category;
            if (category != null)
            {
                var otherPost = _context.Products.Where(p => p.ProductCategoryProducts.Any(c => c.Category.Id == category.Id)) // lấy 5 sản phẩm cùng danh mục
                                            .Where(p => p.ProductId != Post.ProductId)
                                            .OrderByDescending(p => p.DateUpdated)
                                            .Take(5);
                ViewBag.otherPost = otherPost;

            }
           
            

            return View(Post);
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
