using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAppTinhVanCat_aspnetcore.Models.Product;

namespace WebAppTinhVanCat_aspnetcore.Models.Product
{
    [Table("UnitProduct")]
    public class UnitProduct
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Đơn vị")]
        public string Unit { get; set; }

        public ICollection<ProductModel> Products { get; set; }
    }
}
