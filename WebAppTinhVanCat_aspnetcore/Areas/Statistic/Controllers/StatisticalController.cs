using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppTinhVanCat_aspnetcore.Data;

namespace WebAppTinhVanCat_aspnetcore.Areas.Statistic.Controllers
{
    [Area("Statistic")]
    [Route("admin/statistical/[action]/{id?}")]
    [Authorize(Roles = RoleName.Administrator )]
    public class StatisticalController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
