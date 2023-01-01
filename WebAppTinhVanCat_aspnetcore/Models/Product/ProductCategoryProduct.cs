using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppTinhVanCat_aspnetcore.Models.Products
{
    [Table("ProductCategoryProduct")]
    public class ProductCategoryProduct
    {
        // PostID và CategoryID là khóa chính tạo quan hệ nhiều - nhiều cho bản post và category
        public int ProductID { set; get; }

        public int CategoryProductID { set; get; }

        [ForeignKey("ProductID")]
        public virtual ProductModel Product { set; get; }

        [ForeignKey("CategoryProductID")]
        public virtual CategoryProduct Category { set; get; }
    }
}
