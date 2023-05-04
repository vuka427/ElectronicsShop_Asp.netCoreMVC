using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bogus.DataSets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAppTinhVanCat_aspnetcore.Areas.Blog.Models;
using WebAppTinhVanCat_aspnetcore.Data;
using WebAppTinhVanCat_aspnetcore.Models;
using WebAppTinhVanCat_aspnetcore.Models.Blog;
using WebAppTinhVanCat_aspnetcore.Models.Product;
using WebAppTinhVanCat_aspnetcore.Utilities;
using WebAppTinhVanCataspnetcore.Migrations;

namespace WebAppTinhVanCat_aspnetcore.Areas.Blog.Controllers
{
    [Area("Blog")]
    [Route("admin/blog/post/[action]/{id?}")]
    [Authorize(Roles = RoleName.Administrator + "," + RoleName.Editor)]
    public class PostController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _usermanager;

        public PostController(AppDbContext context, UserManager<AppUser> usermanager)
        {
            _context = context;
            _usermanager = usermanager;
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
        public async Task<IActionResult> Create()
        {
            var categories = await _context.Categories.ToListAsync();
            ViewData["categories"] = new MultiSelectList(categories,"Id", "Title");
            
            return View();
        }

        // POST: Blog/Post/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,Slug,Content,Published,CategoryIDs")] CreatePostModel post)
        {
            var categories = await _context.Categories.ToListAsync();
            ViewData["categories"] = new MultiSelectList(categories, "Id", "Title");
            if (post.Slug == null) // phát sinh url nếu chưa có
            {
                post.Slug = AppUtilities.GenerateSlug(post.Title);
            }
            if (await _context.Posts.AnyAsync(p => p.Slug == post.Slug))//kiểm tra url có đc sử dụng hay chưa
            {
                ModelState.AddModelError("Slug", "Nhập chuỗi Url khác !");
                return View(post);
            }

            

            if (ModelState.IsValid)
            {
                var user = await _usermanager.GetUserAsync(this.User);
                post.DateCreated = post.DateUpdated = DateTime.Now;
                post.AuthorId = user.Id;
                _context.Add(post);

                if (post.CategoryIDs != null)
                {
                    foreach (var cate in post.CategoryIDs)
                    {
                        _context.Add(new PostCategory()
                        {
                            CategoryID = cate,
                            Post = post
                        }) ;
                    }
                }

                
                await _context.SaveChangesAsync();
                StatusMessage = "Vừa tạo bài viết mới ";
                return RedirectToAction(nameof(Index));
            }
            
            return View(post);
        }

        // GET: Blog/Post/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.Include(p => p.PostCategories).FirstOrDefaultAsync(p=>p.PostId == id);
            if (post == null)
            {
                return NotFound();
            }
            var PostEdit = new CreatePostModel()
            {
                PostId = post.PostId,
                Title = post.Title,
                Content = post.Content,
                Description = post.Description,
                Slug = post.Slug,
                Published = post.Published,
                CategoryIDs = post.PostCategories.Select(pc => pc.CategoryID).ToArray()
            };

            var categories = await _context.Categories.ToListAsync();
            ViewData["categories"] = new MultiSelectList(categories, "Id", "Title");
            return View(PostEdit);
        }

        // POST: Blog/Post/Edit/5
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostId,Title,Description,Slug,Content,Published,CategoryIDs")] CreatePostModel post)
        {
            if (id != post.PostId)
            {
                return NotFound();
            }
            var categories = await _context.Categories.ToListAsync();
            ViewData["categories"] = new MultiSelectList(categories, "Id", "Title");

            if (post.Slug == null) // phát sinh url nếu chưa có
            {
                post.Slug = AppUtilities.GenerateSlug(post.Title);
            }
            if (await _context.Posts.AnyAsync(p => p.Slug == post.Slug && p.PostId != id))//kiểm tra url có đc sử dụng hay chưa
            {
                ModelState.AddModelError("Slug", "Nhập chuỗi Url khác !");
                return View(post);
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    var PostUpdate = await _context.Posts.Include(p => p.PostCategories).FirstOrDefaultAsync(p => p.PostId == id);
                    if (PostUpdate == null)
                    {
                        return NotFound("lỗi rồi hehe !");
                    }

                    PostUpdate.Title = post.Title;
                    PostUpdate.Content = post.Content;
                    PostUpdate.Description = post.Description;
                    PostUpdate.Slug = post.Slug;
                    PostUpdate.Published = post.Published;
                    PostUpdate.DateUpdated = DateTime.Now;

                    // update categoryIDs
                    if (post.CategoryIDs == null) post.CategoryIDs = new int[] {};

                    var oldCateIds = PostUpdate.PostCategories.Select(c => c.CategoryID).ToArray();//dánh sách cate id cũ
                    var newCateIds = post.CategoryIDs; //dánh sách cate id mới

                    var removeCatePost = from postCate in PostUpdate.PostCategories 
                                         where (!newCateIds.Contains(postCate.CategoryID)) //lấy các postCategory không có trong dánh sách postCategory mới 
                                         select postCate;

                    _context.PostCategories.RemoveRange(removeCatePost);// xóa các postCategory không có trong dánh sách postCategory cập nhật mới

                    var addCateIds = from CateId in newCateIds //danh sách id của các categoy cần cập nhật
                                      where !oldCateIds.Contains(CateId) //lấy các idc trong dánh sách postCategory mới mà không có trong danh sách cũ
                                      select CateId;

                    foreach (var CateId in addCateIds)
                    {
                        _context.PostCategories.Add(new PostCategory()// thêm PostCategory từ danh sách id của các categoy cần cập nhật
                        {
                            PostID = id,
                            CategoryID = CateId
                        });
                    }

                    _context.Update(PostUpdate);
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
                StatusMessage = "Vừa cập nhật bài viết !";
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
            StatusMessage = $"Đã xóa bài viết : {post.Title}!";
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.PostId == id);
        }

        public class UploadOneFile
        {
            [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Phải chọn file Upload")]
            [DataType(DataType.Upload)]
            [FileExtensions(Extensions = "png,jpg,jpeg,gif")]
            [Display(Name = "Chọn file upload ")]
            public IFormFile FileUpload { get; set; }


        }

        [HttpPost]
        public IActionResult ListPhotos(int id)
        {//API get photo 
            var post = _context.Posts.Find(id);
            if (String.IsNullOrEmpty(post.PhotoName))
            {
                return Json(new
                {
                    success = 0,
                    message = "không tìm thấy photo "
                });
            }

            return Json(new
            {
                success = 1,
                postid = id,
                photos = "/contens/Posts/" + post.PhotoName
            });
        }

        [HttpPost]
        public IActionResult DeletePhoto(int id)
        {
            var post = _context.Posts.Find(id);
            if (post != null)
            {
                var filename = "Uploads/Posts/" + post.PhotoName;
                post.PhotoName = "";
                _context.SaveChanges();
                try
                {
                    System.IO.File.Delete(filename);
                }
                catch
                {
                    Console.WriteLine("khong xoa duoc anh tren o dia !");
                }


            }



            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UploadPhotoApi(int id, [Bind("FileUpload")] UploadOneFile f)
        {

            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound("không tìm thấy sản phẩm");

            }


            if (f.FileUpload != null)
            {
                var fileNameRandom = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(f.FileUpload.FileName);//tên file random + extension file upload

                var filePath = Path.Combine("Uploads", "Posts", fileNameRandom); //đường đẫn đến file

                if (!String.IsNullOrEmpty(post.PhotoName))// xóa ảnh nếu có
                {
                    var filename = "Uploads/Posts/" + post.PhotoName;
                    try
                    {
                        System.IO.File.Delete(filename);
                    }
                    catch
                    {
                        Console.WriteLine("khong xoa duoc anh tren o dia !");
                    }
                }

                using (var filestream = new FileStream(filePath, FileMode.Create))
                {
                    await f.FileUpload.CopyToAsync(filestream); // copy file f vào filestream
                }

                
                
                post.PhotoName = fileNameRandom;
                
                _context.Update(post);  
                await _context.SaveChangesAsync();
            }

            return Ok();
        }



    }
}
