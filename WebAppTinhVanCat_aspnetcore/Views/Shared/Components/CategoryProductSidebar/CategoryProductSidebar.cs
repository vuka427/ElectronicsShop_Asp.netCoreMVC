using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebAppTinhVanCat_aspnetcore.Models;
using WebAppTinhVanCat_aspnetcore.Models.Products;

namespace WebAppTinhVanCat_aspnetcore.Components
{
    public class CategoryProductSidebar : ViewComponent
    {


        public class CategoryProductSidebarData 
        {
            public List<CategoryProduct> Categories { get; set; } // tất cả danh mục
            public int Level { get; set; } // danh mục đang truy cập
            public string? CategorySlug { get; set; } // url  đang truy cập 
        }


        public IViewComponentResult Invoke(CategoryProductSidebarData data) // phương thức invoke là bắt buộc , tượng tự như action
        {
            return View(data);
        }
    }
}
