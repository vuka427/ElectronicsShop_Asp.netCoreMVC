using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebAppTinhVanCat_aspnetcore.Models.Blog;

namespace WebAppTinhVanCat_aspnetcore.Areas.Blog.Models
{
    public class CreatePostModel : Post
    {
        [Display(Name ="Chuyên mục")]
        public int[] CategoryIDs { get; set; }

    }
}
