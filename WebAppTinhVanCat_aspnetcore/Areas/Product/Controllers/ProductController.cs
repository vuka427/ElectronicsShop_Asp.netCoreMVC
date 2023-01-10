using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using WebAppTinhVanCat_aspnetcore.Areas.Blog.Models;
using WebAppTinhVanCat_aspnetcore.Areas.Product.Models;
using WebAppTinhVanCat_aspnetcore.Data;
using WebAppTinhVanCat_aspnetcore.Models;
using WebAppTinhVanCat_aspnetcore.Models.Products;
using WebAppTinhVanCat_aspnetcore.Utilities;


namespace WebAppTinhVanCat_aspnetcore.Areas.Product.Controllers
{
    [Area("Product")]
    [Route("admin/productmanage/[action]/{id?}")]
    [Authorize(Roles = RoleName.Administrator + "," + RoleName.Editor)]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _usermanager;

        public ProductController(AppDbContext context, UserManager<AppUser> usermanager)
        {
            _context = context;
            _usermanager = usermanager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        // GET: Blog/Post
        public async Task<IActionResult> Index([FromQuery(Name = "p")] int currentPage , int pagesize)
        {
            var ListProduct = _context.Products.Include(p => p.Author).OrderByDescending(p=>p.DateUpdated);

          

            var totalProduct = await ListProduct.CountAsync(); // tổng số sản phẩm
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
                generateUrl = (int? pagenumber) => Url.Action("Index", new { p = pagenumber, pagesize = pagesize }) //phát url với pagenumber và pagesize
            };

            ViewBag.PagingModel = pagingmodel;
            ViewBag.ProductIndex = (currentPage - 1) * pagesize;
            ViewBag.TotalProduct = totalProduct;

            var ListProductInPage = await ListProduct.Skip((currentPage - 1) * pagesize) // bỏ qua nhưng sản phẩm của trang trước đó
                        .Take(pagesize)// lấy sản phẩm trang hiện tại
                        .Include(p=>p.ProductCategoryProducts)
                        .ThenInclude(pc=>pc.Category)
                        .ToListAsync(); 
          

            return View(ListProductInPage);
        }

        // GET: Blog/Post/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Author)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Blog/Post/Create
        public async Task<IActionResult> Create()
        {
            var categories = await _context.CategoryProducts.ToListAsync();
            ViewData["categories"] = new MultiSelectList(categories,"Id", "Title");
            
            return View();
        }

        // POST: Blog/Post/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Price,Description,Slug,Content,Published,CategoryIDs")] CreateProductModel product)
        {
            var categories = await _context.CategoryProducts.ToListAsync();
            ViewData["categories"] = new MultiSelectList(categories, "Id", "Title");
            if (product.Slug == null) // phát sinh url nếu chưa có
            {
                product.Slug = AppUtilities.GenerateSlug(product.Title);
            }
            if (await _context.CategoryProducts.AnyAsync(p => p.Slug == product.Slug))//kiểm tra url có đc sử dụng hay chưa
            {
                ModelState.AddModelError("Slug", "Nhập chuỗi Url khác !");
                return View(product);
            }

            

            if (ModelState.IsValid)
            {
                var user = await _usermanager.GetUserAsync(this.User);
                product.DateCreated = product.DateUpdated = DateTime.Now;
                product.AuthorId = user.Id;
                _context.Add(product);

                if (product.CategoryIDs != null)
                {
                    foreach (var cate in product.CategoryIDs)
                    {
                        _context.Add(new ProductCategoryProduct()
                        {
                            CategoryProductID = cate,
                            Product = product
                        }) ;
                    }
                }

                
                await _context.SaveChangesAsync();
                StatusMessage = "Vừa Thêm sản phẩm mới ";
                return RedirectToAction(nameof(Index));
            }
            
            return View(product);
        }

        // GET: Blog/Post/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.Include(p => p.ProductCategoryProducts).FirstOrDefaultAsync(p=>p.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            var ProductEdit = new CreateProductModel()
            {
                ProductId = product.ProductId,
                Title = product.Title,
                Price= product.Price,
                Content = product.Content,
                Description = product.Description,
                Slug = product.Slug,
                Published = product.Published,
                CategoryIDs = product.ProductCategoryProducts.Select(pc => pc.CategoryProductID).ToArray()
            };

            var categories = await _context.CategoryProducts.ToListAsync();
            ViewData["categories"] = new MultiSelectList(categories, "Id", "Title");
            return View(ProductEdit);
        }

        // POST: Blog/Post/Edit/5
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Title,Price,Description,Slug,Content,Published,CategoryIDs")] CreateProductModel product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }
            var categories = await _context.CategoryProducts.ToListAsync();
            ViewData["categories"] = new MultiSelectList(categories, "Id", "Title");

            if (product.Slug == null) // phát sinh url nếu chưa có
            {
                product.Slug = AppUtilities.GenerateSlug(product.Title);
            }
            if (await _context.Products.AnyAsync(p => p.Slug == product.Slug && p.ProductId != id))//kiểm tra url có đc sử dụng hay chưa
            {
                ModelState.AddModelError("Slug", "Nhập chuỗi Url khác !");
                return View(product);
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    var ProductUpdate = await _context.Products.Include(p => p.ProductCategoryProducts).FirstOrDefaultAsync(p => p.ProductId == id);
                    if (ProductUpdate == null)
                    {
                        return NotFound("lỗi rồi hehe !");
                    }

                    ProductUpdate.Title = product.Title;
                    ProductUpdate.Price= product.Price;
                    ProductUpdate.Content = product.Content;
                    ProductUpdate.Description = product.Description;
                    ProductUpdate.Slug = product.Slug;
                    ProductUpdate.Published = product.Published;
                    ProductUpdate.DateUpdated = DateTime.Now;

                    // update categoryIDs
                    if (product.CategoryIDs == null) product.CategoryIDs = new int[] {};

                    var oldCateIds = ProductUpdate.ProductCategoryProducts.Select(c => c.CategoryProductID).ToArray();//dánh sách cate id cũ
                    var newCateIds = product.CategoryIDs; //dánh sách cate id mới

                    var removeCatePost = from productCate in ProductUpdate.ProductCategoryProducts
                                         where (!newCateIds.Contains(productCate.CategoryProductID)) //lấy các ProductCategoryProduct không có trong dánh sách ProductCategoryProduct mới 
                                         select productCate;

                    _context.ProductCategoryProducts.RemoveRange(removeCatePost);// xóa các ProductCategoryProduct không có trong dánh sách ProductCategoryProduct cập nhật mới

                    var addCateIds = from CateId in newCateIds //danh sách id của các CategoryProduct cần cập nhật
                                     where !oldCateIds.Contains(CateId) //lấy các idc trong dánh sách ProductCategoryProduct mới mà không có trong danh sách cũ
                                     select CateId;

                    foreach (var CateId in addCateIds)
                    {
                        _context.ProductCategoryProducts.Add(new ProductCategoryProduct()// thêm ProductCategoryProduct từ danh sách id của các CategoryProduct cần cập nhật
                        {
                            ProductID = id,
                            CategoryProductID = CateId
                        });
                    }

                    _context.Update(ProductUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(product.ProductId))
                    {
                        return NotFound("lỗi rồi hehe");
                    }
                    else
                    {
                        throw;
                    }
                }
                StatusMessage = "Vừa cập nhật thông tin sản sản phẩm!";
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", product.AuthorId);
            return View(product);
        }

        // GET: Blog/Post/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Products
                .Include(p => p.Author)
                .FirstOrDefaultAsync(m => m.ProductId == id);
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
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            StatusMessage = $"Đã xóa sản phẩm : { product.Title} !";
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }


        public class UploadOneFile
        {
            [System.ComponentModel.DataAnnotations.Required(ErrorMessage ="Phải chọn file Upload")]
            [DataType(DataType.Upload)]
            [FileExtensions(Extensions ="png,jpg,jpeg,gif")]
            [Display(Name = "Chọn file upload ")]
            public IFormFile FileUpload { get; set; }


        }


        [HttpGet]
        public IActionResult UploadPhoto(int id)
        {

            var product = _context.Products.Where(p=> p.ProductId == id).Include(p=>p.Photos).FirstOrDefault();
            if (product == null)
            {
                return NotFound("không tìm thấy sản phẩm");
                
            }
           

            ViewData["product"] = product;


            return View(new UploadOneFile());
        }

        [HttpPost,ActionName("UploadPhoto") ]
        public async Task<IActionResult> UploadPhotoAsync(int id,[Bind("FileUpload")]  UploadOneFile f)
        {

            var product = _context.Products.Where(p => p.ProductId == id)
                                            .Include(p=>p.Photos)
                                            .FirstOrDefault();

            if (product == null)
            {
                return NotFound("không tìm thấy sản phẩm");

            }
            

            ViewData["product"] = product;

            if (f.FileUpload !=null)
            {
                var fileNameRandom = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(f.FileUpload.FileName);//tên file random + extension file upload

                var filePath = Path.Combine("Uploads", "Products", fileNameRandom); //đường đẫn đến file

                using (var filestream = new FileStream(filePath,FileMode.Create))
                {
                    await f.FileUpload.CopyToAsync(filestream); // copy file f vào filestream
                }

                _context.Add(new ProductPhoto()
                {
                    ProductID = product.ProductId,
                    FileName = fileNameRandom
                });
                await _context.SaveChangesAsync();
            }

            return View(new UploadOneFile());
        }


        [HttpPost]
        public IActionResult ListPhotos(int id) {//API get photo 
            var product = _context.Products.Where(p => p.ProductId == id).Include(p => p.Photos).FirstOrDefault();
            if (product == null)
            {
                return Json(new
                {
                    success = 0,
                    message = "không tìm thấy photo "
                });
            }

            var ListPhotos = product.Photos.Select(photo=> new {

                id = photo.Id,
                path = "/contens/Products/" + photo.FileName

            });


            return Json(new
            {
                success = 1,
                photos = ListPhotos
            });
        }

        [HttpPost]
        public IActionResult DeletePhoto(int id)
        {
            var photo = _context.ProductPhotos.Where(pt=> pt.Id == id).FirstOrDefault();
            if (photo != null)
            {
                var filename = "Uploads/Products/" + photo.FileName;
                _context.Remove(photo);
                _context.SaveChanges();
                try{
                    System.IO.File.Delete(filename);
                }catch
                {
                    Console.WriteLine("khong xoa duoc anh tren o dia !");
                }
                
                
            }



            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UploadPhotoApi(int id, [Bind("FileUpload")] UploadOneFile f)
        {

            var product = _context.Products.Where(p => p.ProductId == id)
                                            .Include(p => p.Photos)
                                            .FirstOrDefault();

            if (product == null)
            {
                return NotFound("không tìm thấy sản phẩm");

            }


            if (f.FileUpload != null)
            {
                var fileNameRandom = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(f.FileUpload.FileName);//tên file random + extension file upload

                var filePath = Path.Combine("Uploads", "Products", fileNameRandom); //đường đẫn đến file

                using (var filestream = new FileStream(filePath, FileMode.Create))
                {
                    await f.FileUpload.CopyToAsync(filestream); // copy file f vào filestream
                }

                _context.Add(new ProductPhoto()
                {
                    ProductID = product.ProductId,
                    FileName = fileNameRandom
                });
                await _context.SaveChangesAsync();
            }

            return Ok();
        }
    }
}
