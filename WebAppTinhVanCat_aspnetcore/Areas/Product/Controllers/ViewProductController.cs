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
using System.Globalization;
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
        public IActionResult Index(string productslug, 
                                    [FromQuery(Name = "p")] int currentPage, 
                                    int pagesize, 
                                    [FromQuery] string searchstring, 
                                    [FromQuery] string sortby, 
                                    [FromQuery] int trademark,
                                    [FromQuery] int pricestart,
                                    [FromQuery] int priceend) 
        {

            ViewBag.categories = GetCategories();//tất cả danh mục
            ViewBag.categoryslug = productslug;//url truy cập hiện tại
            ViewBag.trandemark = _context.TradeMarks.ToList();
            ViewBag.trandemarkID = trademark;
            ViewBag.pricestart = pricestart;
            ViewBag.priceend = priceend;


            CategoryProduct category = null; //danh mục tương ứng với url truy cập hiện tại


            if (!string.IsNullOrEmpty(productslug) ) // nếu tồn tại url chỉ định danh mục thì lấy danh mục tương ứng
            {
                category = _context.CategoryProducts.Where(c=>c.Slug == productslug)
                                                .Include(c => c.CategoryChildren)                    
                                                .FirstOrDefault();
                if(category == null) return NotFound("không tìm thấy danh mục");
            }
            ViewBag.category = category;
            IQueryable<ProductModel> product;
            if (!String.IsNullOrEmpty(searchstring))
            {
                searchstring = searchstring.Trim().ToLower();
                product = _context.Products.Include(p => p.TradeMark).Where(p => p.Title.ToLower().Contains(searchstring) || p.Description.ToLower().Contains(searchstring) || p.TradeMark.Name.ToLower().Contains(searchstring)) //tìm kiếm trên tất cả các sản phẩm
                                                    .Include(p => p.Author)
                                                    .Include(p => p.Photos)
                                                    .Include(p => p.Category)
                                                    .AsQueryable();
            }
            else
            {
                if (trademark>0)
                {
                    product = _context.Products.Where(p=>p.TradeMarkId == trademark).Include(p => p.Author) //tất cả các sản phẩm theo nhãn hiệu
                                                      .Include(p => p.Photos)
                                                      .Include(p => p.Category)
                                                      .AsQueryable();

                }
                else
                {  
                    

                    product = _context.Products.Include(p => p.Author) //tất cả các sản phẩm
                                                      .Include(p => p.Photos)
                                                      .Include(p => p.Category)
                                                      .AsQueryable(); 
                }

            }

            if(priceend > 0 && priceend > pricestart)
            {
                product = product.Where(p=>p.Price >= pricestart && p.Price <= priceend);//sấp xếp sản phẩm
            }

            ViewBag.sortby = sortby;
            if (!String.IsNullOrEmpty(sortby))
            {

                switch (sortby)
                {
                    case "datecreate":
                        product = product.OrderByDescending(p => p.DateUpdated);//sấp xếp sản phẩm
                        break;
                    case "pricedown":
                        product = product.OrderByDescending(p => p.Price);//sấp xếp sản phẩm giá giảm dần 
                        break;
                    case "priceup":
                        product = product.OrderBy(p => p.Price);//sấp xếp sản phẩm giá tăng dần
                        break;
                    default:
                        product = product.OrderByDescending(p => p.DateUpdated);//sấp xếp sản phẩm
                        break;
                }
            }
            else
            {
                product = product.OrderByDescending(p => p.DateUpdated);//sấp xếp sản phẩm
            }
                                    
           


            if (category != null) //nếu có danh mục thì lấy các sản phẩm con cháu và của danh mục đó (lấy sản phẩm theo danh mục)
            {
                var idCateChill = new List<int>();
                category.ChildCategoryIDs(idCateChill, null);// lấy id của tất cả danh mục con
                idCateChill.Add(category.Id);

                product = product.Where(p => idCateChill.Contains(p.CategoryId)); //lấy tất cả cá sản phẩm mà có id danh mục có trong lits idCateChill

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
                generateUrl = (int? pagenumber) => Url.Action("Index", new { p = pagenumber, pagesize = pagesize , sortby = sortby , pricestart = pricestart, priceend = priceend , searchstring = searchstring }) //action phát sinh url
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
                                        .Include(p => p.Category)
                                        .FirstOrDefault();
            if(Product == null)
            {
                return NotFound("không thấy bài viết !");
            }
             
            ViewBag.category = Product.Category;//lấy danh mục của sản phẩm
            ViewBag.Reviews = _context.OrderItems.Where(o=>o.ProductID == Product.ProductId && o.rating!=0).Include(o=>o.Order).ToList();//lấy danh mục của sản phẩm

            if (Product.Category != null)
            {
                var otherProduct = _context.Products.Where(p => p.Category == Product.Category) // lấy 5 sản phẩm cùng danh mục
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
