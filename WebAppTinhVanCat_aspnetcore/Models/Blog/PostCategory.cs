using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppTinhVanCat_aspnetcore.Models.Blog
{
    [Table("PostCategory")]
    public class PostCategory
    {
        // PostID và CategoryID là khóa chính tạo quan hệ nhiều - nhiều cho bản post và category
        public int PostID { set; get; }

        public int CategoryID { set; get; }

        [ForeignKey("PostID")]
        public virtual Post Post { set; get; }

        [ForeignKey("CategoryID")]
        public virtual Category Category { set; get; }
    }
}
