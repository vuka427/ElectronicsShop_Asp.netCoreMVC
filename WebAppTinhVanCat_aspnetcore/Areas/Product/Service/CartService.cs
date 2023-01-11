using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using WebAppTinhVanCat_aspnetcore.Areas.Product.Models;
using WebAppTinhVanCat_aspnetcore.Models;

namespace WebAppTinhVanCat_aspnetcore.Areas.Product.Service
{
    public class CartService
    {
        // Key lưu chuỗi json của Cart
        public const string CARTKEY = "cart";

        private readonly IHttpContextAccessor _context ;
        private readonly HttpContext _httpContext;
        private readonly AppDbContext _DBcontext;

        public CartService(IHttpContextAccessor context, AppDbContext _dbcontext)
        {
            _context = context;
            _httpContext = context.HttpContext;
            _DBcontext = _dbcontext;
        }

        // Lấy cart từ Session (danh sách CartItem)
        public List<CartItem> GetCartItems()
        {

            var session = _httpContext.Session;
            string jsoncart = session.GetString(CARTKEY); // lấy chuổi json với key CARTKEY
            if (jsoncart != null)
            {

                var cart = JsonConvert.DeserializeObject<List<CartItem>>(jsoncart);//khôi phục lại cartItem từ json
                return cart;
            }
            return new List<CartItem>();
        }
        // Lấy cart từ Session (danh sách CartItem)
        public List<CartItem> GetCartItemsWithPhoto()
        {

            var session = _httpContext.Session;
            string jsoncart = session.GetString(CARTKEY); // lấy chuổi json với key CARTKEY
            if (jsoncart != null)
            {

                var cart = JsonConvert.DeserializeObject<List<CartItem>>(jsoncart);//khôi phục lại cartItem từ json
                cart.ForEach(c => c.product = _DBcontext.Products.Where(p => p.ProductId == c.product.ProductId).Include(pt => pt.Photos).FirstOrDefault());
                return cart;
            }
            return new List<CartItem>();
        }

        // Xóa cart khỏi session
        public void ClearCart()
        {
            var session = _httpContext.Session;
            session.Remove(CARTKEY);
        }

        // Lưu Cart (Danh sách CartItem) vào session
        public void SaveCartSession(List<CartItem> ls)
        {
            var session = _httpContext.Session;
            string jsoncart = JsonConvert.SerializeObject(ls);
            session.SetString(CARTKEY, jsoncart);
        }

    }
}
