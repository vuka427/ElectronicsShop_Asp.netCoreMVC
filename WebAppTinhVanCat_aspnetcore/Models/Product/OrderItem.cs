using Bogus.DataSets;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAppTinhVanCat_aspnetcore.Models.Products;

namespace WebAppTinhVanCat_aspnetcore.Models.Product
{
    [Table("OrderItem")]
    public class OrderItem
    {
        public int ProductID { set; get; }

        public int OrderID { get; set; }

        [ForeignKey("ProductID")]
        public ProductModel Products { get; set; }

        [ForeignKey("OrderID")]
        public OrderModel Order { get; set; }

        [Display(Name = "Giá")]
        [Column(TypeName = "decimal(18,4)")] //chỉ định độ chính xác với 18 chữa số trong đó có 4 chữ số thập phân 
        [Range(0, int.MaxValue, ErrorMessage = "Nhập giá trị từ {1} đến {2} ")]
        public decimal CurentPrice { get; set; }

        [Display(Name = "Số lượng ")]
        public int Quantity { get; set; }
    }
}
