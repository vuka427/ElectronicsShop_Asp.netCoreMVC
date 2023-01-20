using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System.Linq;
using WebAppTinhVanCat_aspnetcore.Areas.Product.Models;
using WebAppTinhVanCat_aspnetcore.Areas.Product.Service;
using WebAppTinhVanCat_aspnetcore.Models;
using WebAppTinhVanCat_aspnetcore.Services;

namespace WebAppTinhVanCat_aspnetcore.Areas.Product.Controllers
{
    [Area("Product")]
    public class OrderController : Controller
    {
        private readonly ILogger<ViewProductController> _logger;
        private readonly AppDbContext _context;
        private readonly CartService _cartService;
        private readonly DiaGioiHanhChinhVN _diaGioi;

        public OrderController(ILogger<ViewProductController> logger, AppDbContext context, CartService cartService, DiaGioiHanhChinhVN diaGioi)
        {
            _logger = logger;
            _context = context;
            _cartService = cartService;
            _diaGioi = diaGioi;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("addcart/{productid:int}", Name = "addcart")]
        public IActionResult AddToCart([FromRoute] int productid)
        {

            var product = _context.Products
                .Where(p => p.ProductId == productid)
                .FirstOrDefault();
            if (product == null)
                return NotFound("Không có sản phẩm");

            // Xử lý đưa vào Cart ...
            var cart = _cartService.GetCartItems();
            var cartitem = cart.Find(p => p.product.ProductId == productid);
            if (cartitem != null)
            {
                // Đã tồn tại, tăng thêm 1
                cartitem.quantity++;
            }
            else
            {
                //  Thêm mới
                cart.Add(new CartItem() { quantity = 1, product = product });
            }

            // Lưu cart vào Session
            _cartService.SaveCartSession(cart);
            // Chuyển đến trang hiện thị Cart
            return RedirectToAction(nameof(Cart));
        }

        [Route("/cart", Name = "cart")]
        public IActionResult Cart()
        {
            var cart = _cartService.GetCartItemsWithPhoto();

            

            return View(cart);
        }

        /// Cập nhật
        [Route("/updatecart", Name = "updatecart")]
        [HttpPost]
        public IActionResult UpdateCart([FromForm] int productid, [FromForm] int quantity)
        {
            // Cập nhật Cart thay đổi số lượng quantity ...

            var cart = _cartService.GetCartItems();
            var cartitem = cart.Find(p => p.product.ProductId == productid);
            if (quantity > cartitem.product.Quantity)
            {
                return Ok();//không làm j hết //sử lý sau
            }
            if (cartitem != null)
            {

                // Đã tồn tại, tăng thêm 1
                cartitem.quantity = quantity;
            }
            _cartService.SaveCartSession(cart);
            // Trả về mã thành công (không có nội dung gì - chỉ để Ajax gọi)
            return Ok();
        }

        /// xóa item trong cart
        [Route("/removecart/{productid:int}", Name = "removecart")]
        public IActionResult RemoveCart([FromRoute] int productid)
        {
            var cart = _cartService.GetCartItems();
            var cartitem = cart.Find(p => p.product.ProductId == productid);
            if (cartitem != null)
            {
                // Đã tồn tại, tăng thêm 1
                cart.Remove(cartitem);
            }

            _cartService.SaveCartSession(cart);
            return RedirectToAction(nameof(Cart));
        }

        [Route("/checkout")]
        public IActionResult Checkout()
        {
            var cart = _cartService.GetCartItemsWithPhoto();

            ViewData["ListCartItem"] = cart;

            return View() ;
        }

        [HttpPost]
        [Route("/checkoutconfirm")]
        public IActionResult CheckoutConfirm( [Bind("DcTinh,DcHuyen,DcXa,SoNha,PhoneNumber")] CheckoutModel model)
        {

            if (ModelState.IsValid)
            {
                var Address = _diaGioi.GetAddress(model.DcTinh, model.DcHuyen, model.DcXa);
                var cart = _cartService.GetCartItems();

                if (cart == null)
                {
                    return NotFound("Giỏ hàng trống!");
                }

                bool IsCheckout = true;

                cart.ForEach(cartitem =>
                {
                    if (cartitem.quantity > cartitem.product.Quantity)// sl trông giỏ lớn hơn trong kho
                    {
                        IsCheckout = false;
                    }

                });


               // _cartService.ClearCart();



                return Content("Đặt hàng thành công");
            }

           
            return Content("Có lỗi xảy ra vui lòng thử lại");
        }

        [Route("/admin/ordermanage")]
        public IActionResult OrderManage()
        {
            
            return View();
        }

    }
}
