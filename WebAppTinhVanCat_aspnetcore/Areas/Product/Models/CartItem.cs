using WebAppTinhVanCat_aspnetcore.Models.Products;

namespace WebAppTinhVanCat_aspnetcore.Areas.Product.Models
{
    public class CartItem
    {
        public int quantity { set; get; }
        public ProductModel product { set; get; }
    }
}
