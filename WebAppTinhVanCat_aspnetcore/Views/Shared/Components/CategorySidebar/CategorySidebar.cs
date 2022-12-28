using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebAppTinhVanCat_aspnetcore.Models;

namespace WebAppTinhVanCat_aspnetcore.Components
{
    public class CategorySidebar : ViewComponent
    {


        public class CategorySidebarData 
        {
            public List<Category> Categories { get; set; } // dánh mục
            public int Level { get; set; } // danh mục đang truy cập
            public string CategorySlug { get; set; } // url  đang truy cập 
        }


        public IViewComponentResult Invoke(CategorySidebarData data) // phương thức invoke là bắt buộc , tượng tự như action
        {
            return View(data);
        }
    }
}
