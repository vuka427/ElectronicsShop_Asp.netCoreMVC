using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using WebAppTinhVanCat_aspnetcore.Areas.Product.Models;

namespace WebAppTinhVanCat_aspnetcore.Areas.Product.Service
{
    public class CartService
    {
        // Key lưu chuỗi json của Cart
        public const string CARTKEY = "cart";

        private readonly IHttpContextAccessor _context ;
        private readonly HttpContext _httpContext;

        public CartService(IHttpContextAccessor context)
        {
            _context = context;
            _httpContext = context.HttpContext;
        }

        // Lấy cart từ Session (danh sách CartItem)
        public List<CartItem> GetCartItems()
        {

            var session = _httpContext.Session;
            string jsoncart = session.GetString(CARTKEY);
            if (jsoncart != null)
            {
                return JsonConvert.DeserializeObject<List<CartItem>>(jsoncart);
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
