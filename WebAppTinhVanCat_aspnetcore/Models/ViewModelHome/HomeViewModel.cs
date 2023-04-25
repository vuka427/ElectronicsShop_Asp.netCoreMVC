using System.Collections.Generic;
using WebAppTinhVanCat_aspnetcore.Models.Product;

namespace WebAppTinhVanCat_aspnetcore.Models.ViewModelHome
{
    public class HomeViewModel
    {
        public string Title { get; set; }
        public string Slug { get; set; }

        public List<ProductModel> Products { get; set; }
    }
}
