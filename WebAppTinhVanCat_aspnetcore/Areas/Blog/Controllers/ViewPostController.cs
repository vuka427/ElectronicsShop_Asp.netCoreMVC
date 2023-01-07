using Castle.Core.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using WebAppTinhVanCat_aspnetcore.Models;
using WebAppTinhVanCat_aspnetcore.Models.Blog;

namespace WebAppTinhVanCat_aspnetcore.Areas.Blog.Controllers
{
    [Area("Blog")]
    public class ViewPostController : Controller
    {
        private readonly ILogger<ViewPostController> _logger;
        private readonly AppDbContext _context;

        public ViewPostController(ILogger<ViewPostController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        //post
        //post/{categoryslug?}
        [Route("/post/{categoryslug?}")]
        [AllowAnonymous]
        public IActionResult Index(string categoryslug, [FromQuery(Name = "p")] int currentPage, int pagesize) 
        {
            ViewBag.categories = GetCategories();//tất cả danh mục
            ViewBag.categoryslug = categoryslug;//url truy cập hiện tại

            Category category = null; //danh mục tương ứng với url truy cập hiện tại

            if (!string.IsNullOrEmpty(categoryslug) ) // nếu tồn tại url chỉ định danh mục thì lấy danh mục tương ứng
            {
                category = _context.Categories.Where(c=>c.Slug == categoryslug)
                                                .Include(c => c.CategoryChildren)                    
                                                .FirstOrDefault();
                if(category == null) return NotFound("không tìm thấy danh mục");
            }
            ViewBag.category = category;


            var Post = _context.Posts.Include(p => p.Author) //tất cả các bài viết
                                    .Include(p => p.PostCategories)
                                    .ThenInclude(p => p.Category)
                                    .AsQueryable(); 
                                    
            Post = Queryable.OrderByDescending(Post, p => p.DateUpdated);


            if (category != null) //  nếu có danh mục thì lấy các bài viết con cháu và của danh mục đó
            {
                var ids = new List<int>();
                category.ChildCategoryIDs(ids, null);
                ids.Add(category.Id);

                Post = Post.Where(p => p.PostCategories.Where(pc => ids.Contains(pc.CategoryID)).Any());
                
            }
            // phân trang
            
            var totalPosts = Queryable.Count(Post); // tổng số bài viết
            if (pagesize <= 0) pagesize = 10;
            var countPages = (int)Math.Ceiling((double)totalPosts / pagesize); // tính số lượng trang  =  tổng số bài viết / số bài viết trên 1 trang

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
            ViewBag.PostIndex = (currentPage - 1) * pagesize;
            ViewBag.TotalPost = totalPosts;

            var ListPostInPage = Post.Skip((currentPage - 1) * pagesize) // bỏ qua những bài viết của trang trước đó
                        .Take(pagesize)// lấy bài viết trang hiện tại
                        .ToList();
            

            return View(ListPostInPage);
        }




        [Route("/post/{postslug}.html")]
        [AllowAnonymous]
        public IActionResult Detail(string postslug) 
        {
            ViewBag.categories = GetCategories();//tất cả danh mục


            var Post = _context.Posts.Where(p => p.Slug == postslug)
                                        .Include(p => p.Author)
                                        .Include(p => p.PostCategories)
                                        .ThenInclude(pc => pc.Category)
                                        .FirstOrDefault();
            if(Post == null)
            {
                return NotFound("không thấy bài viết !");
            }
            Category category =  Post.PostCategories.FirstOrDefault()?.Category;//lấy danh mục của bài viết
            ViewBag.category = category;
            if (category != null)
            {
                var otherPost = _context.Posts.Where(p => p.PostCategories.Any(c => c.Category.Id == category.Id)) // lấy 5 bài viết cùng danh mục
                                            .Where(p => p.PostId != Post.PostId)
                                            .OrderByDescending(p => p.DateUpdated)
                                            .Take(5);
                ViewBag.otherPost = otherPost;

            }
           
            

            return View(Post);
        }

        [NonAction]
        private List<Category> GetCategories() // lấy tất cả cây danh mục  
        {

            var categories = _context.Categories
                                    .Include(c => c.CategoryChildren)
                                    .AsEnumerable()
                                    .Where(c=>c.ParentCategory == null)
                                    .ToList();

            return categories;

        }
    }
}
