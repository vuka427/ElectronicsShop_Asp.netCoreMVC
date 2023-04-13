using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppTinhVanCat_aspnetcore.Areas.Product.Models;
using WebAppTinhVanCat_aspnetcore.Areas.Product.Service;
using WebAppTinhVanCat_aspnetcore.Models;
using WebAppTinhVanCat_aspnetcore.Models.Product;
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
        private readonly UserManager<AppUser> _userManager;

        public OrderController(ILogger<ViewProductController> logger, AppDbContext context, CartService cartService, DiaGioiHanhChinhVN diaGioi, UserManager<AppUser> userManager)
        {
            _logger = logger;
            _context = context;
            _cartService = cartService;
            _diaGioi = diaGioi;
            _userManager = userManager;
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
                
                if (cartitem.quantity == cartitem.product.Quantity)// kiểm tra xem số lượng trong đơn hạng có vượt quá số lượng hiện có hay không
                {
                    return RedirectToAction(nameof(Cart));// nếu đặt tới giới hạn rồi thì ko thêm nữa
                }

                // Đã tồn tại, tăng thêm 1
                cartitem.quantity++;
            }
            else
            {
                var productAdd = _context.Products.Find(productid);
                if(productAdd != null)
                {
                    if (productAdd.Quantity > 0 )// kiểm tra xem số lượng hiện còn hay không
                    {
                        //  Thêm mới
                        cart.Add(new CartItem() { quantity = 1, product = product });
                    }
                }
                    
                
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
            
            if (cartitem != null)
            {

                if (quantity > cartitem.product.Quantity)// kiểm tra xem số lượng trong đơn hạng có vượt quá số lượng hiện có hay không
                {
                    return Ok();// không làm j hết
                }
                //  cập nhật mới
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
        public async Task<IActionResult> CheckoutConfirm( [Bind("DcTinh,DcHuyen,DcXa,SoNha,PhoneNumber")] CheckoutModel model)
        {

            if (ModelState.IsValid)
            {
                var Address = _diaGioi.GetAddress(model.DcTinh, model.DcHuyen, model.DcXa); //địa chỉ tỉnh thành
                var cart = _cartService.GetCartItems();// dánh sách hàng hóa 

                if (cart == null)
                {
                    //sử lý trả về lổi 
                    //chưa làm j hết để tạm vậy
                    return NotFound("Giỏ hàng trống!");
                }

                bool IsCheckout = true;// đơn hàng có thể đặt
                decimal totalPrice = 0;

                var user = await GetAppUserAsync(); //lấy user hiện tại

                if ( user == null)
                {
                    //sử lý trả về lổi 
                    //chưa làm j hết để tạm vậy
                    return NotFound(" user!");
                }
                if (user.FullName == null)
                {
                    //sử lý trả về lổi 
                    //chưa làm j hết để tạm vậy

                    return NotFound("có lỗi người dùng chưa Cập nhật tên!");
                }

                var Order = new OrderModel()
                {
                    CustomerID = user.Id,
                    CreateDate = DateTime.Now,
                    State = StateOrder.Received,
                    Address = model.SoNha + ", " + Address,
                    FullName = user.FullName,
                    Email = user.Email,
                    Phone = model.PhoneNumber
                };

                var ListOrderItem = new List<OrderItem>();
                cart.ForEach(c => c.product = _context.Products.Where(p => p.ProductId == c.product.ProductId).Include(pt => pt.UnitProduct).FirstOrDefault());
                cart.ForEach(cartitem =>
                {
                    if (cartitem.quantity > cartitem.product.Quantity)// sl trông giỏ lớn hơn trong kho => ko đặt hàng đc 
                    {
                        IsCheckout = false;
                    }

                    // tính tổng tiền 
                    totalPrice += (cartitem.product.Price * cartitem.quantity);//tạm thời chưa tính thuế
                    ListOrderItem.Add(new OrderItem()
                    {
                        Order = Order,
                        ProductTitle = cartitem.product.Title,
                        ProductID = cartitem.product.ProductId,
                        Quantity = cartitem.quantity,
                        CurentPrice= cartitem.product.Price,
                        GTGT = cartitem.product.GTGT,
                        Unit = cartitem.product.UnitProduct.Unit, //có thể có lỗi
                    });

                });

                Order.Price = totalPrice;
                Order.OrderItems = ListOrderItem;
                
                if (IsCheckout)
                {
                    
                    
                    _context.Add(Order);
                    cart.ForEach(cartitem =>
                    {
                       cartitem.product.Quantity -= cartitem.quantity;
                    });

                    _cartService.ClearCart();
                    _context.SaveChanges();
                    return RedirectToAction("Index","Home");
                }

               
            }

           
            return Content("Có lỗi xảy ra vui lòng thử lại");
        }

        private async Task<AppUser> GetAppUserAsync()
        {
            return await _userManager.GetUserAsync(User);
        }

        [Route("/admin/ordermanage")]
        public IActionResult OrderManage()
        {
            
            return View();
        }

    }
}
