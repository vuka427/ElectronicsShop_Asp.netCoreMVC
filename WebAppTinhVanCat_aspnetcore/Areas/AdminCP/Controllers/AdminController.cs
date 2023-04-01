using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebAppTinhVanCat_aspnetcore.Data;
using WebAppTinhVanCat_aspnetcore.Models;

namespace WebAppTinhVanCat_aspnetcore.Areas.AdminCP.Controllers
{
    [Area("AdminCP")]
    [Authorize(Roles = RoleName.Administrator)]
    public class AdminController : Controller
    {

        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        [Route("/admincp/")]
        public IActionResult Index()
        {

            ViewBag.totalorder = _context.Orders.Where(o=>o.State == Models.Product.StateOrder.Accept).Count();






            return View();
        }
    }
}
