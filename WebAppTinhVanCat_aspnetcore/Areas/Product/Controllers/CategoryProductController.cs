using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAppTinhVanCat_aspnetcore.Models;
using WebAppTinhVanCat_aspnetcore.Models.Products;
using WebAppTinhVanCat_aspnetcore.Data;

namespace WebAppTinhVanCat_aspnetcore.Areas.Product.Controllers
{
    
    [Area("Product")]
    [Route("admin/categoryproduct/[action]/{id?}")]
    [Authorize(Roles = RoleName.Administrator)]
    public class CategoryProductController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryProductController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Blog/Category
        public async Task<IActionResult> Index()
        {

            var qr = (from c in _context.CategoryProducts select c)
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

            var category = await _context.CategoryProducts
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
        private void CreateSelectItems(List<CategoryProduct> source, List<CategoryProduct> des, int level)
        {
            string prefix = string.Concat(Enumerable.Repeat("--- ", level));
            foreach (var category in source)
            {
                
                des.Add(new CategoryProduct() { 
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
            var qr = (from c in _context.CategoryProducts select c)
                                       .Include(c => c.ParentCategory)
                                       .Include(c => c.CategoryChildren);

            var categories = (await qr.ToListAsync()).Where(c => c.ParentCategory == null).ToList();

            categories.Insert(0, new CategoryProduct(){
                    Id = -1,
                    Title = "Không có danh mục cha"
            });

            var Items = new List<CategoryProduct>();

            CreateSelectItems(categories,Items,0 );

            var selectList = new SelectList(Items, "Id", "Title");

            ViewData["ParentCategoryId"] = selectList;


            return View();
        }

        // POST: Blog/Category/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ParentCategoryId,Title,Content,Slug")] CategoryProduct category)
        {
            if (ModelState.IsValid)
            {
                if (category.ParentCategoryId == -1) category.ParentCategoryId = null;
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var qr = (from c in _context.CategoryProducts select c)
                                       .Include(c => c.ParentCategory)
                                       .Include(c => c.CategoryChildren);

            var categories = (await qr.ToListAsync()).Where(c => c.ParentCategory == null).ToList();
            categories.Insert(0, new CategoryProduct()
            {
                Id = -1,
                Title = "Không có danh mục cha"

            });

            var Items = new List<CategoryProduct>();

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

            var category = await _context.CategoryProducts.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var qr = (from c in _context.CategoryProducts select c)
                                      .Include(c => c.ParentCategory)
                                      .Include(c => c.CategoryChildren);

            var categories = (await qr.ToListAsync()).Where(c => c.ParentCategory == null).ToList();
            categories.Insert(0, new CategoryProduct()
            {
                Id = -1,
                Title = "Không có danh mục cha"

            });
            var Items = new List<CategoryProduct>();
            CreateSelectItems(categories, Items, 0);
            var selectList = new SelectList(Items, "Id", "Title");

            ViewData["ParentCategoryId"] = selectList;
            return View(category);
        }

        // POST: Blog/Category/Edit/5
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ParentCategoryId,Title,Content,Slug")] CategoryProduct category)
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
                 var childCategory = (from cate in  _context.CategoryProducts select cate).AsNoTracking() //có lỗi ở đây
                    .Include(c => c.CategoryChildren)
                    .ToList()
                    .Where(c => c.ParentCategoryId == category.Id) ;

                

                //func check id kiểm tra danh mục cha có trùng với con không 
                Func<List<CategoryProduct>, bool> checkParentCate = null;
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
                    
                    _context.CategoryProducts.Update(category);
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

            var qr = (from c in _context.CategoryProducts select c)
                                       .Include(c => c.ParentCategory)
                                       .Include(c => c.CategoryChildren);

            var categories = (await qr.ToListAsync()).Where(c => c.ParentCategory == null).ToList();
            categories.Insert(0, new CategoryProduct()
            {
                Id = -1,
                Title = "Không có danh mục cha"

            });

            var Items = new List<CategoryProduct>();

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

            var category = await _context.CategoryProducts
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
            var category = await _context.CategoryProducts
                            .Include(c => c.CategoryChildren)
                            .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null) return NotFound();

            foreach (var cCategory in category.CategoryChildren)
            {
                cCategory.ParentCategoryId = category.ParentCategoryId;
            }


            _context.CategoryProducts.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.CategoryProducts.Any(e => e.Id == id);
        }
    }
}
