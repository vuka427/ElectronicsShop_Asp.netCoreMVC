using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAppTinhVanCat_aspnetcore.Models;
using WebAppTinhVanCat_aspnetcore.Data;

namespace WebAppTinhVanCat_aspnetcore.Areas.Blog.Controllers
{
    
    [Area("Blog")]
    [Route("admin/blog/category/[action]/{id?}")]
    [Authorize(Roles = RoleName.Administrator)]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Blog/Category
        public async Task<IActionResult> Index()
        {

            var qr = (from c in _context.Categories select c)
                                        .Include(c => c.ParentCategory)
                                        .Include(c => c.CategoryChildren);
            var categories = (await qr.ToListAsync()).Where(c => c.ParentCategory == null).ToList();

            var selectList = new SelectList(categories, "Id", "Title"); 

            return View(categories);
        }

        


        // GET: Blog/Category/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.ParentCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }


        // Source: nguồn 
        // des: Category đã xử lý
        //level: cấp danh mục 
        private void CreateSelectItems(List<Category> source, List<Category> des, int level)
        {
            string prefix = string.Concat(Enumerable.Repeat("--- ", level));
            foreach (var category in source)
            {
                
                des.Add(new Category() { 
                    Id = category.Id,
                    Title = prefix + category.Title
                });
                if(category.CategoryChildren?.Count > 0)
                {
                    CreateSelectItems(category.CategoryChildren.ToList(), des, level+1);
                }
            }
        }

        // GET: Blog/Category/Create
        public async Task<IActionResult> Create()
        {
            var qr = (from c in _context.Categories select c)
                                       .Include(c => c.ParentCategory)
                                       .Include(c => c.CategoryChildren);

            var categories = (await qr.ToListAsync()).Where(c => c.ParentCategory == null).ToList();

            categories.Insert(0, new Category(){
                    Id = -1,
                    Title = "Không có danh mục cha"
            });

            var Items = new List<Category>();

            CreateSelectItems(categories,Items,0 );

            var selectList = new SelectList(Items, "Id", "Title");

            ViewData["ParentCategoryId"] = selectList;


            return View();
        }

        // POST: Blog/Category/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ParentCategoryId,Title,Content,Slug")] Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.ParentCategoryId == -1) category.ParentCategoryId = null;
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var qr = (from c in _context.Categories select c)
                                       .Include(c => c.ParentCategory)
                                       .Include(c => c.CategoryChildren);

            var categories = (await qr.ToListAsync()).Where(c => c.ParentCategory == null).ToList();
            categories.Insert(0, new Category()
            {
                Id = -1,
                Title = "Không có danh mục cha"

            });

            var Items = new List<Category>();

            CreateSelectItems(categories, Items, 0);

            var selectList = new SelectList(Items, "Id", "Title");

            ViewData["ParentCategoryId"] = selectList;

            return View(category);
        }

        // GET: Blog/Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var qr = (from c in _context.Categories select c)
                                      .Include(c => c.ParentCategory)
                                      .Include(c => c.CategoryChildren);

            var categories = (await qr.ToListAsync()).Where(c => c.ParentCategory == null).ToList();
            categories.Insert(0, new Category()
            {
                Id = -1,
                Title = "Không có danh mục cha"

            });
            var Items = new List<Category>();
            CreateSelectItems(categories, Items, 0);
            var selectList = new SelectList(Items, "Id", "Title");

            ViewData["ParentCategoryId"] = selectList;
            return View(category);
        }

        // POST: Blog/Category/Edit/5
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ParentCategoryId,Title,Content,Slug")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            bool CanUpdate = true; // cho phép cập nhật
            

            if (category.ParentCategoryId == category.Id) // không cho phép chọn chính nó là danh mục cha
            {
                ModelState.AddModelError(String.Empty,"Phải chọn danh mục cha khác !");
                CanUpdate = false;
            }


            if (CanUpdate && category.ParentCategoryId != null) // không cho phép chọn con của nó làm danh mục cha
            {
                 var childCategory = (from c in _context.Categories.AsNoTracking() select c)
                    .Include(c => c.CategoryChildren)
                    .ToList()
                    .Where(c => c.ParentCategoryId == category.Id) ;

                //func check id kiểm tra danh mục cha có trùng với con không 
                Func<List<Category>, bool> checkParentCate = null;
                checkParentCate = (childCategory) => {
                    foreach (var cate in childCategory)
                    {
                        if (cate.Id == category.ParentCategoryId) 
                        {
                            CanUpdate = false;
                            ModelState.AddModelError(String.Empty, "Phải chọn danh mục cha khác !");
                            return true;
                        }
                        if (cate.CategoryChildren !=null)
                        {
                            return checkParentCate(cate.CategoryChildren.ToList());//đệ quy kiêm tra các danh mục con cấp nhỏ hơn 
                           
                        }
                    }

                    return false;
                };
                //end func

                checkParentCate(childCategory.ToList());
            }
            

            if (CanUpdate && ModelState.IsValid )
            {
                try
                {
                    if (category.ParentCategoryId == -1) category.ParentCategoryId = null;
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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

            var qr = (from c in _context.Categories select c)
                                       .Include(c => c.ParentCategory)
                                       .Include(c => c.CategoryChildren);

            var categories = (await qr.ToListAsync()).Where(c => c.ParentCategory == null).ToList();
            categories.Insert(0, new Category()
            {
                Id = -1,
                Title = "Không có danh mục cha"

            });

            var Items = new List<Category>();

            CreateSelectItems(categories, Items, 0);

            var selectList = new SelectList(Items, "Id", "Title");

            ViewData["ParentCategoryId"] = selectList;

            return View(category);
        }

        // GET: Blog/Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.ParentCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Blog/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories
                            .Include(c => c.CategoryChildren)
                            .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null) return NotFound();

            foreach (var cCategory in category.CategoryChildren)
            {
                cCategory.ParentCategoryId = category.ParentCategoryId;
            }


            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
