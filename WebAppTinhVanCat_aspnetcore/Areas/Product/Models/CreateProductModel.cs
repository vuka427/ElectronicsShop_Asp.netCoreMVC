using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebAppTinhVanCat_aspnetcore.Models;
using WebAppTinhVanCat_aspnetcore.Models.Product;

namespace WebAppTinhVanCat_aspnetcore.Areas.Product.Models
{
    public class CreateProductModel : ProductModel
    {
        [Display(Name ="Chuyên mục")]
        public int[] CategoryIDs { get; set; }

    }
}
