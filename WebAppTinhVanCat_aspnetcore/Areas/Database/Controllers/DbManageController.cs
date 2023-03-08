using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppTinhVanCat_aspnetcore.Models;
using Microsoft.AspNetCore.Identity;
using WebAppTinhVanCat_aspnetcore.Data;
using Bogus;
using WebAppTinhVanCat_aspnetcore.Models.Product;
using WebAppTinhVanCat_aspnetcore.Models.Blog;
using WebAppTinhVanCat_aspnetcore.Models.Product;

namespace WebAppTinhVanCat_aspnetcore.Areas.Database.Controllers
{
    [AllowAnonymous]
    [Area("Database")]
    [Route("/database-manage/[action]")]
    
    public class DbManageController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;

        public DbManageController(AppDbContext dbContext, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }



        //get : /database-manage/index
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult DeleteDb()
        {
            return View();
        }

        [TempData]
        public string StatusMessage { get; set; }

        [HttpPost]
        public async Task<IActionResult> DeleteDbAsync()
        {
            var success = await _dbContext.Database.EnsureDeletedAsync();

            StatusMessage = success ? "Xóa Database thành công !" : "Không xóa được !";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> MigrateAsync()
        {
            await _dbContext.Database.MigrateAsync();
            StatusMessage = "Tạo Database thành công !";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> SeedDataAsync()
        {
            var rolenames = typeof(RoleName).GetFields().ToList();
            foreach(var r in rolenames)
            {
                var rolename = (string)r.GetRawConstantValue();
                var rfound = await _roleManager.FindByNameAsync(rolename);
                if (rfound == null)
                {
                    await _roleManager.CreateAsync(new IdentityRole(rolename));
                }
            }

            // tạo tài khoản admin
            var useradmin = await _userManager.FindByNameAsync("admin");
            if (useradmin == null)
            {
                useradmin = new AppUser()
                {
                    UserName = "admin",
                    Email = "admin@example.com",
                    EmailConfirmed = true,

                };
                await _userManager.CreateAsync(useradmin, "admin");
              
                await _userManager.AddToRoleAsync(useradmin, RoleName.Administrator);

            }
            var user = _userManager.GetUserAsync(this.User).Result;
            if (user !=null)
            {
                SeedPostCategory();
                SeedProductCategory();
            }
            

            StatusMessage = " Vừa seed Database !";

            return RedirectToAction(nameof(Index));
        }

        //phát sinh dữ liệu mẫu cho post và category sử dụng thư viện Bogus
        private void SeedPostCategory()
        {

            _dbContext.Categories.RemoveRange(_dbContext.Categories.Where(c=>c.Content.Contains("[fakeData]")));
            _dbContext.Posts.RemoveRange(_dbContext.Posts.Where(c => c.Content.Contains("[fakeData]")));
            _dbContext.SaveChanges();

            var fakerCategory = new Faker<Category>(); //faker sinh các giá trị mẫu
            int cm = 1;

            fakerCategory.RuleFor(c => c.Title, fk=> $"CM{cm++} " + fk.Lorem.Sentence(1,2).Trim('.'));
            fakerCategory.RuleFor(c => c.Content, fk =>fk.Lorem.Sentence(5) + "[fakeData]");
            fakerCategory.RuleFor(c => c.Slug, fk =>fk.Lorem.Slug());

            var cate1 = fakerCategory.Generate();
                var cate11 = fakerCategory.Generate();
                var cate12 = fakerCategory.Generate();
            var cate2 = fakerCategory.Generate();
                var cate21 = fakerCategory.Generate();
                    var cate211= fakerCategory.Generate();


            cate11.ParentCategory = cate1;
            cate12.ParentCategory = cate1;
            cate21.ParentCategory = cate2;
            cate211.ParentCategory = cate21;

            var categories = new Category[] { cate1, cate2, cate11, cate12, cate21, cate211 };
            _dbContext.AddRange(categories);

            var rCateIndex = new Random();
            int bv = 1;
            var user = _userManager.GetUserAsync(this.User).Result;

            var fakerPost = new Faker<Post>();
            fakerPost.RuleFor(p=> p.AuthorId, f =>  user.Id);
            fakerPost.RuleFor(p => p.Content, f => f.Lorem.Paragraphs(7) + "[fakeData]" );
            fakerPost.RuleFor(p => p.DateCreated, f => f.Date.Between(new DateTime(2022,1,1), new DateTime(2022,12,12)));
            fakerPost.RuleFor(p => p.Description, f => f.Lorem.Sentences(3));
            fakerPost.RuleFor(p => p.Published, f => true );
            fakerPost.RuleFor(p => p.Slug, f => f.Lorem.Slug());
            fakerPost.RuleFor(p => p.Title, f => $"Bài {bv++} " + f.Lorem.Sentence(3,4).Trim('.'));


            List<Post> posts = new List<Post>();
            List<PostCategory> postCategories = new List<PostCategory>();

            for(int i = 1;i< 40; i++)
            {
                var post = fakerPost.Generate();
                post.DateUpdated = post.DateCreated;
                posts.Add(post);
                postCategories.Add(new PostCategory() { 
                    Post = post,
                    Category = categories[rCateIndex.Next(5)]
                });
            }
            _dbContext.AddRange(posts);
            _dbContext.AddRange(postCategories);



            _dbContext.SaveChanges();




        }


        private void SeedProductCategory()
        {

            _dbContext.CategoryProducts.RemoveRange(_dbContext.CategoryProducts.Where(c => c.Content.Contains("[fakeData]")));
            _dbContext.Products.RemoveRange(_dbContext.Products.Where(c => c.Content.Contains("[fakeData]")));
            _dbContext.SaveChanges();

            var fakerCategory = new Faker<CategoryProduct>(); //faker sinh các giá trị mẫu
            int cm = 1;

            fakerCategory.RuleFor(c => c.Title, fk => $"Nhóm SP {cm++} " + fk.Lorem.Sentence(1, 2).Trim('.'));
            fakerCategory.RuleFor(c => c.Content, fk => fk.Lorem.Sentence(5) + "[fakeData]");
            fakerCategory.RuleFor(c => c.Slug, fk => fk.Lorem.Slug());

            var cate1 = fakerCategory.Generate();
            var cate11 = fakerCategory.Generate();
            var cate12 = fakerCategory.Generate();
            var cate2 = fakerCategory.Generate();
            var cate21 = fakerCategory.Generate();
            var cate211 = fakerCategory.Generate();


            cate11.ParentCategory = cate1;
            cate12.ParentCategory = cate1;
            cate21.ParentCategory = cate2;
            cate211.ParentCategory = cate21;

            var categories = new CategoryProduct[] { cate1, cate2, cate11, cate12, cate21, cate211 };
            _dbContext.CategoryProducts.AddRange(categories);

            var unit = _dbContext.UnitProducts.Where(p => p.Unit == "Tấn").FirstOrDefault();
            if(unit==null) _dbContext.Add(new UnitProduct { Unit = "Tấn" });
            _dbContext.SaveChanges();
            //product
            var rCateIndex = new Random();
            int bv = 1;
            var user = _userManager.GetUserAsync(this.User).Result;

            var fakerProduct = new Faker<ProductModel>();
            fakerProduct.RuleFor(p => p.AuthorId, f => user.Id);
            fakerProduct.RuleFor(p => p.Content, f => f.Commerce.ProductDescription() + "[fakeData]");
            fakerProduct.RuleFor(p => p.DateCreated, f => f.Date.Between(new DateTime(2022, 1, 1), new DateTime(2022, 12, 12)));
            fakerProduct.RuleFor(p => p.Description, f => f.Lorem.Sentences(3));
            fakerProduct.RuleFor(p => p.Published, f => true);
            fakerProduct.RuleFor(p => p.Slug, f => f.Lorem.Slug());
            fakerProduct.RuleFor(p => p.Title, f => $"Sp {bv++} " + f.Commerce.ProductName());
            fakerProduct.RuleFor(p => p.Price, f => int.Parse(f.Commerce.Price(500,1000,0)));

            List<ProductModel> products = new List<ProductModel>();
            List<CategoryProduct> Product_Categories = new List<CategoryProduct>();

            var unit2 = _dbContext.UnitProducts.Where(p => p.Unit == "Tấn").FirstOrDefault();

            for (int i = 1; i < 40; i++)
            {
                var product = fakerProduct.Generate();
                product.DateUpdated = product.DateCreated;
                product.Unit = unit2.Id;
                product.Category = categories[rCateIndex.Next(5)];
                products.Add(product);
                
            }
            _dbContext.Products.AddRange(products);
            _dbContext.AddRange(Product_Categories);


            _dbContext.SaveChanges();


        }

    }
}
