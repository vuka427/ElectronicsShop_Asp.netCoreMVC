using Bogus.DataSets;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using WebAppTinhVanCat_aspnetcore.Models.Product;

namespace WebAppTinhVanCat_aspnetcore.Models.Product
{
    [Table("OrderItem")]
    public class OrderItem
    {
        [Key]
        public int OrderItemID { set; get; }

        public int OrderID { get; set; }

        [ForeignKey("OrderID")]
        public OrderModel Order { get; set; }

        
        [Display(Name = "Tên sản phẩm")]
        [StringLength(160, MinimumLength = 5, ErrorMessage = "{0} dài {1} đến {2}")]
        public string ProductTitle { get; set; }

        public int ProductID { get; set; }

        [Display(Name = "Giá")]
        [Column(TypeName = "decimal(10,0)")] //chỉ định độ chính xác với 10 chữa số trong đó có 0 chữ số thập phân 
        [Range(0, int.MaxValue, ErrorMessage = "Nhập giá trị từ {1} đến {2} ")]
        public decimal CurentPrice { get; set; }

        [Display(Name = "Thuế VAT")]
        public int GTGT { get; set; }

        [Display(Name = "Số lượng ")]
        public int Quantity { get; set; }

        [Display(Name = "Đơn vị")]
        public string Unit { get; set; }

        [Display(Name = "Đánh giá")] 
        [StringLength(256, MinimumLength = 0, ErrorMessage = "{0} dài {1} đến {2}")]
        public string Reviews { get; set; }

        [Range(0, 5, ErrorMessage = "Nhập giá trị từ {1} đến {2} ký tự ")]
        [Display(Name = "đánh giá")]
        public int rating { get; set; }

    }
}
