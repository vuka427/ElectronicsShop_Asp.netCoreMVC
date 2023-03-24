using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppTinhVanCat_aspnetcore.Models.Product
{
    [Table("TradeMark")]
    public class TradeMarkModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} không được bỏ trống")]
        [StringLength(50, ErrorMessage = "{0} dài tối đa {1} ký tự")]
        [Display(Name="Tên thương hiệu")]
        public string Name { get; set; }

        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        public ICollection<ProductModel> Products { get; set; }

    }
}
