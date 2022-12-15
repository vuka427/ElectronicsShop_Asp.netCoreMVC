using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAppTinhVanCat_aspnetcore.Data;
using WebAppTinhVanCat_aspnetcore.Models;

namespace WebAppTinhVanCat_aspnetcore.Areas.Blog.Controllers
{
    [Area("Blog")]
    [Route("admin/blog/post/[action]/{id?}")]
    [Authorize(Roles = RoleName.Administrator + "," + RoleName.Editor)]
    public class PostController : Controller
    {
        private readonly AppDbContext _context;

        public PostController(AppDbContext context)
        {
            _context = context;
        }

        [TempData]
        public string StatusMessage { get; set; }

        // GET: Blog/Post
        public async Task<IActionResult> Index([FromQuery(Name = "p")] int currentPage , int pagesize)
        {
            var ListPost = _context.Posts.Include(p => p.Author).OrderByDescending(p=>p.DateUpdated);

          

            var totalPosts = await ListPost.CountAsync(); // tổng số bài viết
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
                generateUrl = (int? pagenumber) => Url.Action("Index", new { p = pagenumber, pagesize = pagesize }) //phát url với pagenumber và pagesize
            };

            ViewBag.PagingModel = pagingmodel;
            ViewBag.PostIndex = (currentPage - 1) * pagesize;
            ViewBag.TotalPost = totalPosts;

            var ListPostInPage = await ListPost.Skip((currentPage - 1) * pagesize) // bỏ qua nhưng bài viết của trang trước đó
                        .Take(pagesize)// lấy bài viết trang hiện tại
                        .Include(p=>p.PostCategories)
                        .ThenInclude(pc=>pc.Category)
                        .ToListAsync(); 
          

            return View(ListPostInPage);
        }

        // GET: Blog/Post/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Author)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Blog/Post/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Blog/Post/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostId,Title,Description,Slug,Content,Published,AuthorId,DateCreated,DateUpdated")] Post post)
        {
            if (ModelState.IsValid)
            {
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", post.AuthorId);
            return View(post);
        }

        // GET: Blog/Post/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", post.AuthorId);
            return View(post);
        }

        // POST: Blog/Post/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostId,Title,Description,Slug,Content,Published,AuthorId,DateCreated,DateUpdated")] Post post)
        {
            if (id != post.PostId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.PostId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", post.AuthorId);
            return View(post);
        }

        // GET: Blog/Post/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Author)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Blog/Post/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            StatusMessage = $"Đã xóa bài viết : {post.Title.Substring(0,20)}...  !";
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.PostId == id);
        }
    }
}
