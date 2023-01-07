using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppTinhVanCat_aspnetcore.Data;

namespace WebAppTinhVanCat_aspnetcore.Areas.AdminCP.Controllers
{
    [Area("AdminCP")]
    [Authorize(Roles = RoleName.Administrator)]
    public class AdminController : Controller
    {
        
        [Route("/admincp/")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
